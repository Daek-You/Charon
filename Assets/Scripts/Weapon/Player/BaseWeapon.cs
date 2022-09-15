using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public string WeaponName { get { return weaponName; } }
    public float DefaultAttackDamage { get { return defaultAttackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }

    [Header("기본 정보 옵션")]
    [SerializeField] protected string weaponName;
    [SerializeField] protected float defaultAttackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;

    public abstract void Attack();
    public abstract void DashAttack();
    public abstract void ChargingAttack();
    public abstract void Skill();
    public abstract void UltimateSkill();
}
