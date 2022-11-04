using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class WeaponManager
{
    public BaseWeapon Weapon { get; private set; }
    public Action<GameObject> unRegisterWeapon { get; set; }
    private Transform handPosition;
    [SerializeField]
    private GameObject weaponObject;
    private List<GameObject> weapons = new List<GameObject>();

    public WeaponManager(Transform hand)
    {
        handPosition = hand;
    }

    public void RegisterWeapon(GameObject weapon)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].name.Equals(weapon.name))
                return;
        }

        BaseWeapon weaponInfo = weapon.GetComponent<BaseWeapon>();
        weapon.transform.SetParent(handPosition);
        weapon.transform.localPosition = weaponInfo.HandleData.localPosition;
        weapon.transform.localEulerAngles = weaponInfo.HandleData.localRotation;
        weapon.transform.localScale = weaponInfo.HandleData.localScale;
        weapons.Add(weapon);
        weapon.SetActive(false);
    }

    public void UnRegisterWeapon(GameObject weapon)
    {
        if (weapons.Contains(weapon))
        {
            weapons.Remove(weapon);
            unRegisterWeapon.Invoke(weapon);
        }
    }

    public void SetWeapon(GameObject weapon)
    {
        BaseWeapon weaponInfo = weapon.GetComponent<BaseWeapon>();
        if (!Player.Instance._AnimationEventHandler.myWeaponEffects.ContainsKey(weaponInfo.Name))
        {
            Player.Instance._AnimationEventHandler.myWeaponEffects.Add(weaponInfo.Name, weapon.GetComponent<IEffect>());
        }

        if (Weapon == null)
        {
            weaponObject = weapon;
            Weapon = weaponInfo;
            weaponObject.SetActive(true);
            Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
            return;
        }

        for(int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].name.Equals(Weapon.name))
            {
                weaponObject = weapons[i];
                weaponObject.SetActive(true);
                Weapon = weapons[i].GetComponent<BaseWeapon>();
                Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
                continue;
            }
            weapons[i].SetActive(false);
        }
    }

    public string GetWeaponName()
    {
        return Weapon.name;
    }
}
