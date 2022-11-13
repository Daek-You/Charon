using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Status : UI_Popup
{
    enum Texts
    {
        UI_StHealth,
        UI_StHealLv,
        HealthCost,
        UI_StAmmor,
        UI_StAmLv,
        ArmorCost,
        UI_StSpeed,
        UI_StSpLv,
        SpeedCost,
        UI_StDash,
        UI_StDashLv,
        DashCountCost
    }

    enum Buttons
    {
        UI_UpBtn_Health,
        UI_UpBtn_Ammor,
        UI_UpBtn_Speed,
        UI_UpBtn_Dash,
        Button_Close
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GameObject button = GetButton((int)Buttons.UI_UpBtn_Health).gameObject;
        BindEvent(button, OnUpgradeMaxHP, UIEvent.Click);
        button = GetButton((int)Buttons.UI_UpBtn_Ammor).gameObject;
        BindEvent(button, OnUpgradeArmor, UIEvent.Click);
        button = GetButton((int)Buttons.UI_UpBtn_Speed).gameObject;
        BindEvent(button, OnUpgradeMoveSpeed, UIEvent.Click);
        button = GetButton((int)Buttons.UI_UpBtn_Dash).gameObject;
        BindEvent(button, OnUpgradeDashCount, UIEvent.Click);

        RefreshUI();
    }

    public void RefreshUI()
    {
        // 현재 강화 수치를 받아와서 초기화
        GetText((int)Texts.UI_StHealth).text = $"체력 +{StatManager.Instance.CurHPLevel}";
        GetText((int)Texts.UI_StAmmor).text = $"방어력 +{StatManager.Instance.CurArmorLevel}";
        GetText((int)Texts.UI_StSpeed).text = $"이동속도 +{StatManager.Instance.CurSpeedLevel}";
        GetText((int)Texts.UI_StDash).text = $"대쉬횟수 +{StatManager.Instance.CurDashLevel}";

        // 현재 수치 값을 받아와서 초기화
        GetText((int)Texts.UI_StHealLv).text = StatManager.Instance.currentMaxHP.ToString();
        GetText((int)Texts.UI_StAmLv).text = StatManager.Instance.currentArmor.ToString();
        GetText((int)Texts.UI_StSpLv).text = StatManager.Instance.currentMoveSpeed.ToString();
        GetText((int)Texts.UI_StDashLv).text = StatManager.Instance.currentDashCount.ToString();

        // 비용 초기화
        if (StatManager.Instance.CurHPLevel < 10)
            GetText((int)Texts.HealthCost).text = DataManager.MaxHpDict[StatManager.Instance.CurHPLevel].currentCost.ToString();
        else
            GetText((int)Texts.HealthCost).text = "-";

        if (StatManager.Instance.CurArmorLevel < 10)
            GetText((int)Texts.ArmorCost).text = DataManager.ArmorDict[StatManager.Instance.CurArmorLevel].currentCost.ToString();
        else
            GetText((int)Texts.ArmorCost).text = "-";

        if (StatManager.Instance.CurSpeedLevel < 10)
            GetText((int)Texts.SpeedCost).text = DataManager.MoveSpeedDict[StatManager.Instance.CurSpeedLevel].currentCost.ToString();
        else
            GetText((int)Texts.SpeedCost).text = "-";

        if (StatManager.Instance.CurDashLevel < 1)
            GetText((int)Texts.DashCountCost).text = DataManager.DashCountDict[StatManager.Instance.CurDashLevel].currentCost.ToString();
        else
            GetText((int)Texts.DashCountCost).text = "-";
    }

    public void OnUpgradeMaxHP(PointerEventData data)
    {
        if (StatManager.Instance.Gold < DataManager.MaxHpDict[StatManager.Instance.CurHPLevel].currentCost)
            return;

        StatManager.Instance.Gold -= DataManager.MaxHpDict[StatManager.Instance.CurHPLevel].currentCost;
        StatManager.Instance.UpgradeStatus(StatType.MAX_HP);
        RefreshUI();
    }

    public void OnUpgradeArmor(PointerEventData data)
    {
        if (StatManager.Instance.Gold < DataManager.ArmorDict[StatManager.Instance.CurArmorLevel].currentCost)
            return;

        StatManager.Instance.Gold -= DataManager.ArmorDict[StatManager.Instance.CurArmorLevel].currentCost;
        StatManager.Instance.UpgradeStatus(StatType.ARMOR);
        RefreshUI();
    }

    public void OnUpgradeMoveSpeed(PointerEventData data)
    {
        if (StatManager.Instance.Gold < DataManager.MoveSpeedDict[StatManager.Instance.CurSpeedLevel].currentCost)
            return;

        StatManager.Instance.Gold -= DataManager.MoveSpeedDict[StatManager.Instance.CurSpeedLevel].currentCost;
        StatManager.Instance.UpgradeStatus(StatType.MOVE_SPEED);
        RefreshUI();
    }

    public void OnUpgradeDashCount(PointerEventData data)
    {
        if (StatManager.Instance.Gold < DataManager.DashCountDict[StatManager.Instance.CurDashLevel].currentCost)
            return;

        StatManager.Instance.Gold -= DataManager.DashCountDict[StatManager.Instance.CurDashLevel].currentCost;
        StatManager.Instance.UpgradeStatus(StatType.DASH_COUNT);
        RefreshUI();
    }
}
