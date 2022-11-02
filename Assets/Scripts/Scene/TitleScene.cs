using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : BaseScene
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        UIManager.Instance.ShowSceneUI<UI_Title>();
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeScene, OnChangeScene);
        StageManager.Instance.CurrentStage = StageType.Unknown;

        GameObject player = GameObject.Find("Sondol");
        if (player != null)
            player.transform.position = new Vector3(0, 0, -20);
    }

    public override void Clear()
    {
        UIManager.Instance.Clear();
    }
}
