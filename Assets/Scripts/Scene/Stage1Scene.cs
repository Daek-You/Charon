using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Scene : BaseScene
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        UIManager.Instance.ShowSceneUI<UI_InGame>();
        UIManager.Instance.ShowSceneUI<UI_AchievementCompletionNotifier>();
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeScene, OnChangeScene);

        // 불러오기인지 판단
        StageManager.Instance.CurrentStage = StageType.Stage11;
        StageManager.Instance.SetStage();
    }

    public override void Clear()
    {
        UIManager.Instance.Clear();
    }
}
