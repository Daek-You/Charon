using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CheckUpgrade : UI_Popup
{
    enum GameObjects
    {
        GridPanel
    }

    static bool isUpgradeOpen = false;
    public static bool IsUpgradeOpen { get { return isUpgradeOpen; } }

    int numOfStats = 5;
    private List<StatType> statList = new List<StatType>();

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        isUpgradeOpen = false;
    }

    public override void Init()
    {
        base.Init();

        isUpgradeOpen = true;
        Bind<GameObject>(typeof(GameObjects));

        GameObject panel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in panel.transform)
            Utils.Destroy(child.gameObject);

        statList = StatManager.Instance.GetStatList();

        for (int i = 0; i < numOfStats; i++)
        {
            GameObject slot = Utils.Instantiate("UI/SubItem/UI_StatSlot");
            slot.transform.SetParent(panel.transform);

            UI_StatSlot statSlot = Utils.GetAddedComponent<UI_StatSlot>(slot);
            statSlot.Type = statList[i];
        }
    }
}
