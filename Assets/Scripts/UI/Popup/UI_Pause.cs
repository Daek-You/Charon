using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Pause : UI_Popup
{
    static bool isPauseOpen = false;
    public static bool IsPauseOpen { get { return isPauseOpen; } }

    enum GameObjects
    {
        BackgroundPanel
    }

    enum Texts
    {
        TxtPause,
        TxtCancel,
        TxtSave,
        TxtSetting,
        TxtQuit
    }

    enum Images
    {
        ImgCancel,
        ImgSave,
        ImgSetting,
        ImgQuit
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        isPauseOpen = false;
    }

    public override void Init()
    {
        base.Init();

        isPauseOpen = true;

        Bind<GameObject>(typeof(GameObjects));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        GameObject go = GetImage((int)Images.ImgCancel).gameObject;
        BindEvent(go, OnCancelPause, UIEvent.Click);
        go = GetImage((int)Images.ImgSave).gameObject;
        BindEvent(go, OnSave, UIEvent.Click);
        go = GetImage((int)Images.ImgSetting).gameObject;
        BindEvent(go, OnSetting, UIEvent.Click);
        go = GetImage((int)Images.ImgQuit).gameObject;
        BindEvent(go, OnQuit, UIEvent.Click);
    }

    public void OnCancelPause(PointerEventData data)
    {
        UIManager.Instance.ClosePopupUI();
    }

    public void OnSave(PointerEventData data)
    {
        UI_SaveGame popup = UIManager.Instance.ShowPopupUI<UI_SaveGame>("UI_StartGame");
    }

    public void OnSetting(PointerEventData data)
    {
        UIManager.Instance.ShowPopupUI<UI_Setting>();
    }

    public void OnQuit(PointerEventData data)
    {
        // 추후에 InGameScene에서 AddListener 해줘야함
        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeScene, this, "TitleScene");
    }
}
