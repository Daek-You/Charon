using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class QuestManager : MonoBehaviour
{
    static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (applicationQuitting)
                return null;

            Init();
            return instance;
        }
    }

    private static object _lock = new object();
    private static bool applicationQuitting = false;

    #region Events
    public delegate void QuestRegisteredHandler(Quest newQuest);
    public delegate void QuestCompletedHandler(Quest quest);
    public delegate void QuestCanceledHandler(Quest quest);

    public event QuestRegisteredHandler onQuestRegistered;
    public event QuestCompletedHandler onQuestCompleted;
    public event QuestCanceledHandler onQuestCanceled;

    public event QuestRegisteredHandler onAchievementRegistered;
    public event QuestCompletedHandler onAchievementCompleted;
    #endregion

    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();

    private List<Quest> activeAchivements = new List<Quest>();
    private List<Quest> completedAchivements = new List<Quest>();

    private static QuestDatabase questDatabase;
    private static QuestDatabase achievementDatabase;

    public IReadOnlyList<Quest> ActiveQuests => activeQuests;
    public IReadOnlyList<Quest> CompletedQuests => completedQuests;

    public IReadOnlyList<Quest> ActiveAchivements => activeAchivements;
    public IReadOnlyList<Quest> CompletedAchivements => completedAchivements;

    private const string saveRootPath = "questSystem";
    private const string activeQuestsSavePath = "activeQuests";
    private const string completedQuestsSavePath = "completedQuests";
    private const string activeAchievementsSavePath = "activeAchievements";
    private const string completedAchievementsSavePath = "completedAchievements";

    private void Start()
    {
        Init();

        if (!LoadQuestData())
        {
            foreach (var achievement in achievementDatabase.Quests)
                Register(achievement);
        }
    }

    private void OnApplicationQuit()
    {
        SaveQuestData();
    }

    private void OnDestroy()
    {
        applicationQuitting = true;
    }

    static void Init()
    {
        lock (_lock)
        {
            if (instance == null)
            {
                GameObject questManager = GameObject.Find("@Quest_Manager");

                if (questManager == null)
                {
                    questManager = new GameObject("@Quest_Manager");
                    questManager.AddComponent<QuestManager>();
                }

                DontDestroyOnLoad(questManager);
                instance = questManager.GetComponent<QuestManager>();

                questDatabase = Resources.Load<QuestDatabase>("Achievement/QuestDatabase");
                achievementDatabase = Resources.Load<QuestDatabase>("Achievement/AchievementDatabase");
            }
        }
    }

    public void SaveQuestData()
    {
        var root = new JObject();
        root.Add(activeQuestsSavePath, CreateSaveDatas(activeQuests));
        root.Add(completedQuestsSavePath, CreateSaveDatas(completedQuests));
        root.Add(activeAchievementsSavePath, CreateSaveDatas(activeAchivements));
        root.Add(completedAchievementsSavePath, CreateSaveDatas(completedAchivements));

        PlayerPrefs.SetString(saveRootPath, root.ToString());
        PlayerPrefs.Save();

        Debug.Log("Quest Datas are saved.");
    }

    public bool LoadQuestData()
    {
        if (PlayerPrefs.HasKey(saveRootPath))
        {
            var root = JObject.Parse(PlayerPrefs.GetString(saveRootPath));

            LoadSaveDatas(root[activeQuestsSavePath], questDatabase, LoadActiveQuest);
            LoadSaveDatas(root[completedQuestsSavePath], questDatabase, LoadCompletedQuest);

            LoadSaveDatas(root[activeAchievementsSavePath], achievementDatabase, LoadActiveQuest);
            LoadSaveDatas(root[completedAchievementsSavePath], achievementDatabase, LoadCompletedQuest);

            return true;
        }

        return false;
    }

    private JArray CreateSaveDatas(IReadOnlyList<Quest> quests)
    {
        var saveDatas = new JArray();
        foreach(var quest in quests)
        {
            if (quest.IsSavable)
                saveDatas.Add(JObject.FromObject(quest.SaveData()));
        }
        return saveDatas;
    }

    private void LoadSaveDatas(JToken datasToken, QuestDatabase database, System.Action<QuestData, Quest> onSuccess)
    {
        var datas = datasToken as JArray;
        foreach (var data in datas)
        {
            var saveData = data.ToObject<QuestData>();
            var quest = database.FindQuestBy(saveData.codeName);
            onSuccess.Invoke(saveData, quest);
        }
    }

    private void LoadActiveQuest(QuestData saveData, Quest quest)
    {
        var newQuest = Register(quest);
        newQuest.LoadData(saveData);
    }

    private void LoadCompletedQuest(QuestData saveData, Quest quest)
    {
        var newQuest = quest.Clone();
        newQuest.LoadData(saveData);

        if (newQuest is Achievement)
            completedAchivements.Add(newQuest);
        else
            completedQuests.Add(newQuest);
    }

    public Quest Register(Quest quest)
    {
        var newQuest = quest.Clone();

        if (newQuest is Achievement)
        {
            newQuest.onCompleted += OnAchivementCompleted;
            activeAchivements.Add(newQuest);
            newQuest.OnRegister();
            onAchievementRegistered?.Invoke(newQuest);
        }
        else
        {
            newQuest.onCompleted += OnQuestCompleted;
            newQuest.onCanceled += OnQuestCanceled;
            activeQuests.Add(newQuest);
            newQuest.OnRegister();
            onQuestRegistered?.Invoke(newQuest);
        }

        return newQuest;
    }

    private void ReceiveReport(List<Quest> quests, string category, object target, int successCount)
    {
        foreach (var quest in quests.ToArray())
            quest.ReceiveReport(category, target, successCount);
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        ReceiveReport(activeQuests, category, target, successCount);
        ReceiveReport(activeAchivements, category, target, successCount);
    }

    public void ReceiveReport(Category category, TaskTarget target, int successCount)
        => ReceiveReport(category.CodeName, target.Value, successCount);

    public bool ContainsInActiveQuests(Quest quest)
    => activeQuests.Any(x => x.CodeName == quest.CodeName);

    public bool ContainsInCompletedQuests(Quest quest)
        => completedQuests.Any(x => x.CodeName == quest.CodeName);

    public bool ContainsInActiveAchievements(Quest achievement)
        => activeAchivements.Any(x => x.CodeName == achievement.CodeName);

    public bool ContainsInCompletedAchievements(Quest achievement)
        => completedAchivements.Any(x => x.CodeName == achievement.CodeName);

    #region Callback
    private void OnQuestCompleted(Quest quest)
    {
        activeQuests.Remove(quest);
        completedQuests.Add(quest);

        onQuestCompleted?.Invoke(quest);
    }

    private void OnQuestCanceled(Quest quest)
    {
        activeQuests.Remove(quest);
        onQuestCanceled?.Invoke(quest);
        Destroy(quest, Time.deltaTime);
    }

    private void OnAchivementCompleted(Quest achivement)
    {
        activeAchivements.Remove(achivement);
        completedAchivements.Add(achivement);

        onAchievementCompleted?.Invoke(achivement);
    }
    #endregion
}
