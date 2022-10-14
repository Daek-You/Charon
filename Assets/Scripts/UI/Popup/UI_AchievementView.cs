using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_AchievementView : UI_Popup
{
    static bool isAchievementOpen = false;
    public static bool IsAchievementOpen { get { return isAchievementOpen; } }

    private int panelCount;
    private Quest target;
    private Dictionary<Task, string> taskDescriptionDict = new Dictionary<Task, string>();

    enum GameObjects
    {
        Content
    }

    enum Texts
    {
        TxtAchievementView,
        TxtAchievementName,
        TxtAchievementDescription,
        TxtTaskDescription
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        isAchievementOpen = false;

        if (target != null)
            target.onTaskSuccessChanged -= UpdateDescription;
    }

    public override void Init()
    {
        base.Init();

        isAchievementOpen = true;
        panelCount = 0;

        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));

        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.CheckAchievement, OnCheckAchievementDetails);

        GameObject panel = Get<GameObject>((int)GameObjects.Content);
        foreach (Transform child in panel.transform)
            Utils.Destroy(child.gameObject);

        var achieveManager = QuestManager.Instance;
        CreateDetailViews(achieveManager.ActiveAchivements, panel);
        CreateDetailViews(achieveManager.CompletedAchivements, panel);

        // 업적 개수에 따라 스크롤을 위한 Contents 크기를 조절
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(350, panelCount * 110 + 10);
    }

    private void CreateDetailViews(IReadOnlyList<Quest> achievements, GameObject parent)
    {
        foreach (var achievement in achievements)
        {
            GameObject detail = Utils.Instantiate("UI/SubItem/UI_AchievementDetailView");
            detail.transform.SetParent(parent.transform);

            UI_AchievementDetailView detailSlot = Utils.GetAddedComponent<UI_AchievementDetailView>(detail);
            detailSlot.Setup(achievement);

            panelCount++;
        }
    }

    public void OnCheckAchievementDetails(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        if (target != null)
            target.onTaskSuccessChanged -= UpdateDescription;
        if (taskDescriptionDict.Count > 0)
            taskDescriptionDict.Clear();

        target = param as Quest;

        GetText((int)Texts.TxtAchievementName).text = target.DisplayName;
        GetText((int)Texts.TxtAchievementDescription).text = target.Description;

        for (int i = 0; i < target.TaskGroups[0].Tasks.Count; i++)
        {
            var task = target.CurrentTaskGroup.Tasks[i];
            taskDescriptionDict.Add(task, BuildTaskDescription(task));
        }

        BuildAchievementDescription();

        if (!target.IsCompleted)
            target.onTaskSuccessChanged += UpdateDescription;
    }

    // 문자열 관련 함수, 최적화 필요함
    private void BuildAchievementDescription()
    {
        string text = "";
        foreach (KeyValuePair<Task, string> item in taskDescriptionDict)
        {
            text += item.Value;
            text += '\n';
        }
        text = text.Substring(0, text.Length - 1);

        GetText((int)Texts.TxtTaskDescription).text = text;
    }

    private string BuildTaskDescription(Task task)
        => $"{task.Description} {task.CurrentSuccess}/{task.NeedSuccessToComplete}";

    private void UpdateDescription(Quest achievement, Task task, int curSuccess, int preSuccess)
    {
        taskDescriptionDict[task] = BuildTaskDescription(task);
        BuildAchievementDescription();
    }
}
