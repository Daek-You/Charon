using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI_Title : UI_Scene
{
    enum Texts
    {
        TxtNewStart,
        TxtContinue,
        TxtSetting,
        TxtQuit
    }

    enum Images
    {
        ImgLogo,
        ImgNewStart,
        ImgContinue,
        ImgSetting,
        ImgQuit
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GameObject go = GetImage((int)Images.ImgNewStart).gameObject;
        BindEvent(go, OnNewStart, UIEvent.Click);
        go = GetImage((int)Images.ImgContinue).gameObject;
        BindEvent(go, OnContinue, UIEvent.Click);
        go = GetImage((int)Images.ImgSetting).gameObject;
        BindEvent(go, OnSetting, UIEvent.Click);
        go = GetImage((int)Images.ImgQuit).gameObject;
        BindEvent(go, OnQuit, UIEvent.Click);
    }

    public void OnNewStart(PointerEventData data)
    {
        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "NewGameScene");
    }

    public void OnContinue(PointerEventData data)
    {
        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "ContinueScene");
    }

    public void OnSetting(PointerEventData data)
    {
        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "SettingScene");
    }

    public void OnQuit(PointerEventData data)
    {
        Debug.Log("종료하기");
        Application.Quit();
    }
}
