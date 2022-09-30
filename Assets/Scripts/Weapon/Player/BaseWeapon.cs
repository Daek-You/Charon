using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseWeapon : MonoBehaviour
{
    public WeaponHandleData HandleData { get { return weaponhandleData; } }
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }
    public string Name { get { return _name; } }
    public float AttackDamage { get { return attackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }


    [Header("생성 정보")]
    [SerializeField] protected WeaponHandleData weaponhandleData;

    [Header("무기 정보")]
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
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

    public abstract void Attack(PlayerController controller);
    public abstract void DashAttack(PlayerController controller);
    public abstract void ChargingAttack(PlayerController controller);
    public abstract void Skill(PlayerController controller);
    public abstract void UltimateSkill(PlayerController controller);
}
