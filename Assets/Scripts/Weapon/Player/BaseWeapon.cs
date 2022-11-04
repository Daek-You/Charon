using CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseWeapon : MonoBehaviour
{
    public int ComboCount { get; set; }
    public int CurrentSkillGauge { get; set; } = 0;
    public const int MAX_SKILL_GAUGE = 10;
    public WeaponHandleData HandleData { get { return weaponhandleData; } }
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }
    public string Name { get { return _name; } }
    public float AttackDamage { get { return attackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }
    private Coroutine checkAttackReInputCor;


    #region #무기 정보
    [Header("생성 정보"), Tooltip("해당 무기를 쥐었을 때의 Local Transform 값 정보입니다.")]
    [SerializeField] protected WeaponHandleData weaponhandleData;

    [Header("무기 정보")]
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

    public void CheckAttackReInput(float reInputTime)
    {
        if (checkAttackReInputCor != null)
            StopCoroutine(checkAttackReInputCor);
        checkAttackReInputCor = StartCoroutine(CheckAttackReInputCoroutine(reInputTime));
    }

    private IEnumerator CheckAttackReInputCoroutine(float reInputTime)
    {
        float currentTime = 0f;
        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= reInputTime)
                break;
            yield return null;
        }

        ComboCount = 0;
        Player.Instance.animator.SetInteger("AttackCombo", 0);
    }
}
