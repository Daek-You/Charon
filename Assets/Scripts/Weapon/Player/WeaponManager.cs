using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{

    public BaseWeapon myWeapon;
    public Vector3 mousePosition { get; private set; }


    public void ChangeWeapon(BaseWeapon otherWeapon)
    {
        myWeapon = otherWeapon;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)  // 이번 프레임에 좌클릭을 했다면
        {

        }
    }
}
