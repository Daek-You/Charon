using CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseWeapon : MonoBehaviour
{
    public int ComboCount { get; set; }
    public WeaponHandleData HandleData { get { return weaponhandleData; } }
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }
    public string Name { get { return _name; } }
    public float AttackDamage { get { return attackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }

    #region #���� ����
    [Header("���� ����"), Tooltip("�ش� ���⸦ ����� ���� Local Transform �� �����Դϴ�.")]
    [SerializeField] protected WeaponHandleData weaponhandleData;

    [Header("���� ����")]
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected string _name;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;
    #endregion

    public void SetWeaponData(string name, float attackDamage, float attackSpeed, float attackRange)
    {
        this._name = name;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }

    public abstract void Attack(BaseState state);
    public abstract void DashAttack(BaseState state);
    public abstract void ChargingAttack(BaseState state);
    public abstract void Skill(BaseState state);
    public abstract void UltimateSkill(BaseState state);
}
