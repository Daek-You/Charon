using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InGame : UI_Scene
{
    enum GameObjects
    {
        SliHP,
        SliST
    }

    enum Images
    {
        ImgPlayer,
        ImgFrame,
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

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        // UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeWeapon, OnChangeWaepon);
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeHP, OnChangeHP);
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeST, OnChangeST);
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeGoods, OnChangeGoods);
    }

    //public void OnChangeWaepon(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    //{
    //    Sprite sprite = Utils.Load<Sprite>($"WeaponTest/{param}");
    //    GetImage((int)Images.ImgWeapon).sprite = sprite;
    //}

    public void OnChangeHP(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        Slider hpBar = GetObject((int)GameObjects.SliHP).GetComponent<Slider>();
        hpBar.value = (float)param;
    }

    public void OnChangeST(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        Slider stBar = GetObject((int)GameObjects.SliST).GetComponent<Slider>();
        stBar.value = (float)param;
    }

    public void OnChangeGoods(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        Get<TextMeshProUGUI>((int)Texts.TxtGoods).text = $"{param}";
    }
}
