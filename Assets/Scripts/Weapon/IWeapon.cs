using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void Attack();
    void DashAttack();
    void ChargeAttack();
    void Skill();
    void UltimateSkill();

}


public class CharonPaddle : IWeapon
{

    public void Attack()
    {

    }

    public void DashAttack()
    {

    }

    public void ChargeAttack()
    {

    }

    public void Skill()
    {

    }

    public void UltimateSkill()
    {

    }


}


public class WeaponManager : MonoBehaviour
{

    public IWeapon currentMyWeapon { get; private set; }



    /// - ���� ü����()
    /// - ���� Ż���ϱ�
    /// - ���� �����ϱ�
}