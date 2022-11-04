using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject weapon;

    void Start()
    {
        GameObject weaponObj = Instantiate(weapon);
        Player.Instance.weaponManager.RegisterWeapon(weaponObj);
        Player.Instance.weaponManager.SetWeapon(weaponObj);
    }
}
