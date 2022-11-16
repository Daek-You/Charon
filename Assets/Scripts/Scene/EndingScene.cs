using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingScene : BaseScene
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        UIManager.Instance.ShowSceneUI<UI_Ending>();
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeScene, OnChangeScene);
        CreateEventSystem();

        StageManager.Instance.CurrentStage = StageType.Ending;
    }

    public override void Clear()
    {
        UIManager.Instance.Clear();
    }
}
