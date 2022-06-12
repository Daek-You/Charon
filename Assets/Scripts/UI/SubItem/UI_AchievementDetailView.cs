using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_AchievementDetailView : UI_Base
{
    private Achievement target;

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
            target.onComplete -= ShowCompletionScreen;
        }
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));

        GetObject((int)GameObjects.ImgComplete).SetActive(false);
    }

    public void Setup(Achievement achievement)
    {
        Init();

        target = achievement;

        GetText((int)Texts.TxtAchievementName).text = achievement.DisplayName;

        var task = achievement.CurrentTaskGroup.Tasks[0];
        GetText((int)Texts.TxtAchievementDetails).text = BuildTaskDescription(task);

        if (achievement.IsComplete)
            GetObject((int)GameObjects.ImgComplete).SetActive(true);
        else
        {
            GetObject((int)GameObjects.ImgComplete).SetActive(false);
            achievement.onTaskSuccessChanged += UpdateDescription;
            achievement.onComplete += ShowCompletionScreen;
        }
    }

    private string BuildTaskDescription(Task task)
        => $"{task.Description} {task.CurrentSuccess}/{task.NeedSuccessToComplete}";

    private void UpdateDescription(Achievement achievement, Task task, int curSuccess, int preSuccess)
        => GetText((int)Texts.TxtAchievementDetails).text = BuildTaskDescription(task);

    private void ShowCompletionScreen(Achievement achievement)
        => GetObject((int)GameObjects.ImgComplete).SetActive(true);
}
