using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : Interactable
{
    [SerializeField]
    private string weaponName;
    [SerializeField]
    private string weaponNameToKorean;
    GameObject weapon;

    private void Start()
    {
        targetText = $"{weaponNameToKorean}";
        UIManager.Instance.MakeWorldSpaceUI<UI_Interaction3D>(transform);
    }

    public override void interact()
    {
        // 무기 교체
        weapon = Utils.Instantiate($"Weapons/{weaponName}");
        Player.Instance.weaponManager.RegisterWeapon(weapon);
        Player.Instance.weaponManager.SetWeapon(weapon);
        Utils.Destroy(weapon);
    }
}
