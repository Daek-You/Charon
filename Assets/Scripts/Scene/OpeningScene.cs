using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningScene : BaseScene
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeScene, OnChangeScene);
        StageManager.Instance.CurrentStage = StageType.Unknown;
    }

    public override void Clear()
    {
        UIManager.Instance.Clear();
    }
}
