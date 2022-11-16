using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Jinkwang_Weapon : EnemyWeapon
{
    public int AttackAnimation { get; set; }

    void Start()
    {
        originalDamage = attackDamage;
    }

    public override void Attack()
    {
        owner.animator.SetBool(AttackAnimation, true);
    }

    public override void StopAttack()
    {
        owner.animator.SetBool(AttackAnimation, false);
    }
}
