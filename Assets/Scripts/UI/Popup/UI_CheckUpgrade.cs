using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CheckUpgrade : UI_Popup
{
    int numOfStats = 5;

    enum GameObjects
    {
        GridPanel
    }

    static bool isUpgradeOpen = false;
    public static bool IsUpgradeOpen { get { return isUpgradeOpen; } }

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        isUpgradeOpen = true;
    }

    private void OnDisable()
    {
        isUpgradeOpen = false;
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject panel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform child in panel.transform)
            Utils.Destroy(child.gameObject);

        // StatManager의 GetStat으로 Dictionary를 반환, 해당 Dict로 초기 강화 레벨 표시
        for (int i = 0; i < numOfStats; i++)
        {
            GameObject slot = Utils.Instantiate("UI/SubItem/UI_StatSlot");
            slot.transform.SetParent(panel.transform);

            UI_StatSlot statSlot = Utils.GetAddedComponent<UI_StatSlot>(slot);
        }
    }
}
