using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_AchievementDetailView : UI_Base
{
    private Quest target;
    private Dictionary<Task, string> taskDescriptionDict = new Dictionary<Task, string>();

    enum GameObjects
    {
        ImgComplete
    }

    enum Texts
    {
        TxtAchievementName,
        TxtAchievementDetails
    }

    private void OnDestroy()
    {
        if (target != null)
        {
            target.onTaskSuccessChanged -= UpdateDescription;
            target.onCompleted -= ShowCompletionScreen;
        }
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetObject((int)GameObjects.ImgComplete).SetActive(false);
    }

    public void Setup(Quest achievement)
    {
        Init();

        target = achievement;

        GetText((int)Texts.TxtAchievementName).text = achievement.DisplayName;

        // TaskGroup이 없을 때, 즉 연계 퀘스트가 없을 때만 기능
        for (int i = 0; i < achievement.TaskGroups[0].Tasks.Count; i++)
        {
            var task = achievement.CurrentTaskGroup.Tasks[i];
            taskDescriptionDict.Add(task, BuildTaskDescription(task));
        }

        BuildAchievementDescription();

        if (achievement.IsCompleted)
            GetObject((int)GameObjects.ImgComplete).SetActive(true);
        else
        {
            GetObject((int)GameObjects.ImgComplete).SetActive(false);
            achievement.onTaskSuccessChanged += UpdateDescription;
            achievement.onCompleted += ShowCompletionScreen;
        }
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

        GetText((int)Texts.TxtAchievementDetails).text = text;
    }

    private string BuildTaskDescription(Task task)
        => $"{task.Description} {task.CurrentSuccess}/{task.NeedSuccessToComplete}";

    private void UpdateDescription(Quest achievement, Task task, int curSuccess, int preSuccess)
    {
        taskDescriptionDict[task] = BuildTaskDescription(task);
        BuildAchievementDescription();
    }

    private void ShowCompletionScreen(Quest achievement)
        => GetObject((int)GameObjects.ImgComplete).SetActive(true);
}
