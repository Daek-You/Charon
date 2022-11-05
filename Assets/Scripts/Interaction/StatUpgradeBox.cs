using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatUpgradeBox : Interactable
{
    private void Start()
    {
        targetText = "Ω∫≈» ∞≠»≠";
        UIManager.Instance.MakeWorldSpaceUI<UI_Interaction3D>(transform);
    }

    public override void interact()
    {
        // Ω∫≈» ∞≠»≠ UI √‚∑¬
        Debug.Log("Ω∫≈» ∞≠»≠ UI");
    }
}
