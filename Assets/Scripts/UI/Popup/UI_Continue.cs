using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Continue : UI_ManageData
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        SetNamespace("�̾��ϱ�");
        SetSaveSlot(DataState.Continue);
    }
}
