using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SaveGame : UI_ManageData
{
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        SetNamespace("�����ϱ�");
        SetSaveSlot(DataState.SaveGame);
    }
}
