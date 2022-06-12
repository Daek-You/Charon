using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_NewGame : UI_ManageData
{

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        SetNamespace("새로하기");
        SetSaveSlot(DataState.NewGame);
    }
}
