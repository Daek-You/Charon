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

    private void OnDestroy()
    {
        if (UIManager.Instance != null)
        {
            UIManager.EventHandler.RemoveEvent(UI_EventHandler.UIEventType.ChangeHP);
            UIManager.EventHandler.RemoveEvent(UI_EventHandler.UIEventType.ChangeST);
            UIManager.EventHandler.RemoveEvent(UI_EventHandler.UIEventType.ChangeGoods);
        }
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeHP, OnChangeHP);
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeST, OnChangeST);
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeGoods, OnChangeGoods);

        Slider hpBar = GetObject((int)GameObjects.SliHP).GetComponent<Slider>();
        hpBar.value = Player.Instance.InitHpBar();

        Slider stBar = GetObject((int)GameObjects.SliST).GetComponent<Slider>();
        stBar.value = Player.Instance.weaponManager.Weapon.InitStBar();

        GetText((int)Texts.TxtGoods).text = StatManager.Instance.Gold.ToString();
        GetImage((int)Images.ImgGoods).GetComponent<RectTransform>().anchoredPosition = new Vector3(-80 - (GetText((int)Texts.TxtGoods).text.Length * 25), 56, 0);
    }

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
        GetImage((int)Images.ImgGoods).GetComponent<RectTransform>().anchoredPosition = new Vector3(-60 - (GetText((int)Texts.TxtGoods).text.Length * 30), 56, 0);
    }
}
