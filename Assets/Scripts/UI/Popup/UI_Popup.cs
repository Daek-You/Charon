using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        UIManager.Instance.SetCanvas(gameObject, true);
    }

    public void ClosePopupUI()
    {
        UIManager.Instance.ClosePopupUI();
    }
}
