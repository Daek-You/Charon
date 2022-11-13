using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UpgradeWeapon : UI_Popup
{
    enum Images
    {
        IMG_WeaponToShow
    }

    enum Texts
    {
        TxtNameToShow,
        UI_StPaddle,
        UI_StPaddleLv,
        UI_StPaddleCost
    }
    
    enum Buttons
    {
        UI_UpBtn_Paddle
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject button = GetButton((int)Buttons.UI_UpBtn_Paddle).gameObject;
        BindEvent(button, OnUpgradePaddle, UIEvent.Click);

        GetText((int)Texts.TxtNameToShow).text = Player.Instance.weaponManager.Weapon.Name;
        GetImage((int)Images.IMG_WeaponToShow).sprite = Resources.Load<Sprite>($"WeaponIcon/{Player.Instance.weaponManager.GetWeaponName()}");
        RefreshUI();
    }

    public void RefreshUI()
    {
        // ��ȭ��ġ, ����(+0.2), ��� ����
    }

    public void OnUpgradePaddle(PointerEventData data)
    {
        // ��ȭ üũ �� ���

        // ���� ��ȭ
        RefreshUI();
    }
}
