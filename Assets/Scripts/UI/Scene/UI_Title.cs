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
        UIManager.Instance.ShowPopupUI<UI_NewGame>("UI_StartGame");
    }

    public void OnContinue(PointerEventData data)
    {
        UIManager.Instance.ShowPopupUI<UI_Continue>("UI_StartGame");
    }

    public void OnSetting(PointerEventData data)
    {
        UIManager.Instance.ShowPopupUI<UI_Setting>();
    }

    public void OnQuit(PointerEventData data)
    {
        Debug.Log("종료하기");
        Application.Quit();
    }
}
