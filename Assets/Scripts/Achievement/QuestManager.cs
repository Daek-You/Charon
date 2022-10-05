using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    static QuestManager instance;
    public static QuestManager Instance { get { Init(); return instance; } }

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

    private QuestDatabase questDatabase;
    private QuestDatabase achievementDatabase;

    public IReadOnlyList<Quest> ActiveQuests => activeQuests;
    public IReadOnlyList<Quest> CompletedQuests => completedQuests;

    public IReadOnlyList<Quest> ActiveAchivements => activeAchivements;
    public IReadOnlyList<Quest> CompletedAchivements => completedAchivements;

    private void Start()
    {
        Init();

        questDatabase = Resources.Load<QuestDatabase>("Achievement/QuestDatabase");
        achievementDatabase = Resources.Load<QuestDatabase>("Achievement/AchievementDatabase");

        foreach (var achievement in achievementDatabase.Quests)
            Register(achievement);
    }

    static void Init()
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
        }
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
