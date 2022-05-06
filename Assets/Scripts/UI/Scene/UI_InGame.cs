using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InGame : UI_Scene
{
    enum Images
    {
        ImgWeapon,
        ImgHP,
        ImgST,
        ImgGoods
    }

    enum Texts
    {
        TxtGoods
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeWeapon, OnChangeWaepon);
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeHP, OnChangeHP);
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeST, OnChangeST);
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeGoods, OnChangeGoods);
    }

    public void OnChangeWaepon(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        Sprite sprite = Utils.Load<Sprite>($"WeaponTest/{param}");
        GetImage((int)Images.ImgWeapon).sprite = sprite;
    }

    public void OnChangeHP(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        Debug.Log("Change HP");
    }

    public void OnChangeST(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        Debug.Log("Change ST");
    }

    public void OnChangeGoods(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        Get<TextMeshProUGUI>((int)Texts.TxtGoods).text = $"{param}";
    }
}
