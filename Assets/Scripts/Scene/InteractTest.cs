using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTest : BaseScene
{
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        UIManager.Instance.ShowSceneUI<UI_InGame>();
        // UIManager.Instance.ShowSceneUI<UI_Interaction>();
        StageManager.Instance.CurrentStage = StageType.Unknown;

        Player.Instance.transform.position = new Vector3(0, 0, 0);
        GameObject weapon = Utils.Instantiate($"Weapons/CharonPaddle");
        Player.Instance.weaponManager.RegisterWeapon(weapon);
        Player.Instance.weaponManager.SetWeapon(weapon);
    }

    public override void Clear()
    {
        UIManager.Instance.Clear();
    }
}
