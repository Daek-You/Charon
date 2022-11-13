using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatSlot : UI_Base
{
    enum Images
    {
        ImgStat
    }

    enum Texts
    {
        TxtUpgradeCount
    }

    private StatType type;
    public StatType Type { get { return type; } set { type = value; } }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeUpgrade, OnCheckUpgrade);

        switch (type)
        {
            case StatType.MAX_HP:
                GetImage((int)Images.ImgStat).sprite = Resources.Load<Sprite>("StatIcon/maxHp");
                GetText((int)Texts.TxtUpgradeCount).text = $"+{StatManager.Instance.CurHPLevel}";
                break;
            case StatType.ARMOR:
                GetImage((int)Images.ImgStat).sprite = Resources.Load<Sprite>("StatIcon/armor");
                GetText((int)Texts.TxtUpgradeCount).text = $"+{StatManager.Instance.CurArmorLevel}";
                break;
            case StatType.MOVE_SPEED:
                GetImage((int)Images.ImgStat).sprite = Resources.Load<Sprite>("StatIcon/moveSpeed");
                GetText((int)Texts.TxtUpgradeCount).text = $"+{StatManager.Instance.CurSpeedLevel}";
                break;
            case StatType.DASH_COUNT:
                GetImage((int)Images.ImgStat).sprite = Resources.Load<Sprite>("StatIcon/dashCount");
                GetText((int)Texts.TxtUpgradeCount).text = $"+{StatManager.Instance.CurDashLevel}";
                break;
            case StatType.PADDLE_DAMAGE:
                break;
        }
    }

    public void OnCheckUpgrade(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        if (!type.Equals((StatType)param))
            return;

        switch (type)
        {
            case StatType.MAX_HP:
                GetText((int)Texts.TxtUpgradeCount).text = $"+{StatManager.Instance.CurHPLevel}";
                break;
            case StatType.ARMOR:
                GetText((int)Texts.TxtUpgradeCount).text = $"+{StatManager.Instance.CurArmorLevel}";
                break;
            case StatType.MOVE_SPEED:
                GetText((int)Texts.TxtUpgradeCount).text = $"+{StatManager.Instance.CurSpeedLevel}";
                break;
            case StatType.DASH_COUNT:
                GetText((int)Texts.TxtUpgradeCount).text = $"+{StatManager.Instance.CurDashLevel}";
                break;
            case StatType.PADDLE_DAMAGE:
                break;
        }
    }
}
