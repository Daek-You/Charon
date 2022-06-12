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

        SetNamespace("저장하기");
        SetSaveSlot(DataState.SaveGame);
    }
}
