using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour
{
    public float AttackDamage { get { return attackDamage; } }
    [SerializeField] protected Enemy owner;
    [SerializeField] protected float attackDamage;



    public abstract void Attack();
    public abstract void StopAttack();
}
