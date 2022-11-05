using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Interaction : UI_Scene
{
    enum Objects
    {
        BackgroundPanel
    }

    enum Texts
    {
        TxtInteraction,
        TxtInteractionButton
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(Objects));
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ShowUI, OnShowUI);
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.HideUI, OnHideUI);

        gameObject.SetActive(false);
    }

    public void OnShowUI(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        GetText((int)Texts.TxtInteraction).text = (string)param;
        GetObject((int)Objects.BackgroundPanel).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(GetText((int)Texts.TxtInteraction).text.Length * 80 + 220, 150);

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
           
    }

    public void OnHideUI(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
