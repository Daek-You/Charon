using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour
{
    public float AttackDamage { get { return attackDamage; } set { attackDamage = value; } }
    [SerializeField] protected Enemy owner;
    [SerializeField] protected float attackDamage;
    public float originalDamage { get; protected set; }



    public abstract void Attack();
    public abstract void StopAttack();

}
