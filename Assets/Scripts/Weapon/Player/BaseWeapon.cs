using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseWeapon : MonoBehaviour
{
    public WeaponHandleData HandleData { get { return weaponhandleData; } }
    public string Name { get { return _name; } }
    public float AttackDamage { get { return attackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }


    [Header("생성 정보")]
    [SerializeField] protected WeaponHandleData weaponhandleData;

    [Header("무기 정보")]
    [SerializeField] protected string _name;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;

    public void SetWeaponData(string name, float attackDamage, float attackSpeed, float attackRange)
    {
        this._name = name;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }

    public abstract void Attack();
    public abstract void DashAttack();
    public abstract void ChargingAttack();
    public abstract void Skill();
    public abstract void UltimateSkill();
}
