using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameScene : BaseScene
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        UIManager.Instance.ShowSceneUI<UI_NewGame>("UI_StartGame");
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeScene, OnChangeScene);
    }

    public override void Clear()
    {
        UIManager.Instance.Clear();
    }
}
