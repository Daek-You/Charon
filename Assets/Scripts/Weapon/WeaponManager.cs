using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager
{

    public IWeapon currentMyWeapon { get; private set; }
    public bool IsChangingWeapon { get; private set; } = false;

    public void SetWeapon(IWeapon weapon)
    {
        if (!IsChangingWeapon)
            currentMyWeapon = weapon;
    }


    /// - ���� ü����()
    /// - ���� Ż���ϱ�
}
