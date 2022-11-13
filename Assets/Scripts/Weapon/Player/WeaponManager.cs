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
    public Dictionary<string, int> reinforceDict = new Dictionary<string, int>();

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
        weaponInfo.Reporter = Utils.GetAddedComponent<QuestReporter>(weapon);
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
            Player.Instance._AnimationEventHandler.mySounds.Add(weaponInfo.Name, weapon.GetComponent<ISound>());
        }

        if (Weapon == null)
        {
            weaponObject = weapon;
            Weapon = weaponInfo;
            weaponObject.SetActive(true);
            Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
            LoadWeaponReinforceInfo();
            return;
        }

        for(int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].name.Equals(weapon.name))
            {
                SaveWeaponReinforceInfo();

                weaponObject = weapons[i];
                weaponObject.SetActive(true);
                Weapon = weapons[i].GetComponent<BaseWeapon>();
                Player.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;

                LoadWeaponReinforceInfo();
                continue;
            }
            weapons[i].SetActive(false);
        }
    }

    public string GetWeaponName()
    {
        // 오브젝트명 (Ex. CharonPaddle)
        return Weapon.name;
    }

    public void SaveWeaponReinforceInfo()
    {
        if (reinforceDict.ContainsKey(Weapon.name))
            reinforceDict[Weapon.name] = Weapon.CurrentReinforceLevel;
        else
            reinforceDict.Add(Weapon.name, Weapon.CurrentReinforceLevel);
    }

    public void LoadWeaponReinforceInfo()
    {
        if (reinforceDict.ContainsKey(Weapon.name))
            Weapon.CurrentReinforceLevel = reinforceDict[Weapon.name];
    }

    public List<string> GetReinforceList()
    {
        SaveWeaponReinforceInfo();
        return new List<string>(reinforceDict.Keys);
    }

    public List<int> GetReinforceValueList()
    {
        return new List<int>(reinforceDict.Values);
    }

    public void LoadWeaponDictionary()
    {
        List<string> keys = DataManager.Instance.SaveData.ReinforceWeaponList;
        List<int> values = DataManager.Instance.SaveData.ReinforceWeaponValueList;
        Dictionary<string, int> dict = new Dictionary<string, int>();

        for (int i = 0; i < keys.Count; i++)
            dict.Add(keys[i], values[i]);

        reinforceDict = dict;
    }
}
