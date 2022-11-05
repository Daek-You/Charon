using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Private_K_Weapon : EnemyWeapon
{
    public readonly int attackAnimation = Animator.StringToHash("IsAttack");

    public override void Attack()
    {
        owner.animator.SetBool(attackAnimation, true);
    }

    public override void StopAttack()
    {
        owner.animator.SetBool(attackAnimation, false);
    }
}
