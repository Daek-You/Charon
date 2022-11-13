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
        GetText((int)Texts.UI_StPaddle).text = $"카론의 노 +{Player.Instance.weaponManager.reinforceDict["CharonPaddle"]}";
        GetText((int)Texts.UI_StPaddleLv).text = $"공격력: {Player.Instance.weaponManager.Weapon.AttackDamage}";
        if (Player.Instance.weaponManager.reinforceDict["CharonPaddle"] < 10)
            GetText((int)Texts.UI_StPaddleCost).text = DataManager.CharonPaddleData[Player.Instance.weaponManager.reinforceDict["CharonPaddle"]].currentCost.ToString();
        else
            GetText((int)Texts.UI_StPaddleCost).text = "-";
    }

    public void OnUpgradePaddle(PointerEventData data)
    {
        if (StatManager.Instance.Gold < DataManager.CharonPaddleData[Player.Instance.weaponManager.reinforceDict["CharonPaddle"]].currentCost)
            return;
        StatManager.Instance.Gold -= DataManager.CharonPaddleData[Player.Instance.weaponManager.reinforceDict["CharonPaddle"]].currentCost;

        StatManager.Instance.UpgradeStatus(StatType.PADDLE_DAMAGE);
        RefreshUI();
    }
}
