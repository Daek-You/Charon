using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUpgradeBox : Interactable
{
    private void Start()
    {
        targetText = "���� ��ȭ";
        UIManager.Instance.MakeWorldSpaceUI<UI_Interaction3D>(transform);
    }

    public override void interact()
    {
        // ���� ��ȭ UI ���
        Debug.Log("���� ��ȭ UI");
    }
}
