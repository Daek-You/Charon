using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueScene : BaseScene
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        UIManager.Instance.ShowSceneUI<UI_Continue>("UI_StartGame");
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeScene, OnChangeScene);
    }

    public override void Clear()
    {
        UIManager.Instance.Clear();
    }
}
