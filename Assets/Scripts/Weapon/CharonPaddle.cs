using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CharonPaddle : MonoBehaviour, IWeapon
{

    public int attackDamage { get; private set; }
    public float attackSpeed { get; private set; }

    private Coroutine dashAttackCor;

    public CharonPaddle(int attackDamage, float attackSpeed)
    {
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
    }

    public void Attack(Controller owner, Vector3 mouseDirection)
    {

    }

    public void DashAttack(Controller owner, Vector3 mouseDirection)
    {
        if (mouseDirection != Vector3.zero)
        {
            IEnumerator dashAttckCoroutine = owner.thePhysics.DashAttackCor(owner, mouseDirection);
            
            if(dashAttckCoroutine != null)
                StopCoroutine(dashAttckCoroutine);
            
            dashAttackCor = StartCoroutine(dashAttckCoroutine);
        }
    }

    public void ChargeAttack(Controller owner, Vector3 mouseDirection)
    {

    }

    public void Skill(Controller owner, Vector3 mouseDirection)
    {

    }

    public void UltimateSkill(Controller owner, Vector3 mouseDirection)
    {

    }


}