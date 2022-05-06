using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingScene : BaseScene
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        UIManager.Instance.ShowSceneUI<UI_Setting>();
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeScene, OnChangeScene);
    }

    public override void Clear()
    {
        UIManager.Instance.Clear();
    }
}
