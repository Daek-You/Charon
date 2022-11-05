using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : BaseScene
{
    float timer = 0f;

    void Start()
    {
        Init();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 4f)
        {
            UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "LobbyScene");
        }

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
