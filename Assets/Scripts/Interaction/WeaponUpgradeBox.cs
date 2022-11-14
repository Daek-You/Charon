using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeBox : Interactable
{
    private void Start()
    {
        targetText = "���� ��ȭ";
        UIManager.Instance.MakeWorldSpaceUI<UI_Interaction3D>(transform);
    }

    public override void interact()
    {
        UIManager.Instance.ShowPopupUI<UI_UpgradeWeapon>();
    }
}
