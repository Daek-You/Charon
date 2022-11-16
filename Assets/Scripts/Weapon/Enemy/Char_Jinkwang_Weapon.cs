using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Jinkwang_Weapon : EnemyWeapon
{
    private int attackAnimation = Animator.StringToHash("IsAttack");
    public int AttackAnimation { get { return attackAnimation; } set { attackAnimation = value; } }

    public override void Attack()
    {
        owner.animator.SetBool(attackAnimation, true);
    }

    public override void StopAttack()
    {
        owner.animator.SetBool(attackAnimation, false);
    }
}
