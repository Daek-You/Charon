using CharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharonPaddle : BaseWeapon, IEffect, ISound
{
    #region #이펙트
    [SerializeField] private GameObject[] defaultAttackEffs;
    [SerializeField] private GameObject dashAttackEffs;
    [SerializeField] private GameObject chargingEffs;
    [SerializeField] private GameObject chargingAttackEffs;
    [SerializeField] private GameObject skillEffs;
    [SerializeField] private GameObject deleteChargingEffs { get; set; }      // 차징 중 다른 상태로 넘어갈 때 제거
    #endregion

    #region #애니메이션
    public readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public readonly int hashAttackAnimation = Animator.StringToHash("AttackCombo");
    public readonly int hashAttackSpeedAnimation = Animator.StringToHash("AttackSpeed");
    public readonly int hashDashAttackAnimation = Animator.StringToHash("IsDashAttack");
    public readonly int hashCharingAttackAnimation = Animator.StringToHash("IsCharingAttack");
    public readonly int hashSkillAnimation = Animator.StringToHash("IsSkill");
    #endregion

    #region
    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private AudioClip chargingSound;
    [SerializeField] private AudioClip chargingAttackSound;
    [SerializeField] private AudioClip skillSound;
    #endregion

    public const float dashAttackPower = 4f;
    private Coroutine dashAttackCoroutine;
    private WaitForSeconds dashAttackSecond = new WaitForSeconds(0.2f);

    public override void Attack(BaseState state)
    {
        ComboCount++;
        MultiplierDamage = 1f;
        VCam.Instance.SetImpulseOptions(gain: 0.25f, amplitude: 1f, frequency: 1, duration: 0.5f);
        Player.Instance.animator.SetFloat(hashAttackSpeedAnimation, attackSpeed);
        Player.Instance.animator.SetBool(hashIsAttackAnimation, true);
        Player.Instance.animator.SetInteger(hashAttackAnimation, ComboCount);
        CheckAttackReInput(AttackState.CanReInputTime);

        float knockBackGauge = BaseWeapon.DEFAULT_KNOCKBACK_POWER;
        switch (ComboCount)
        {
            case 2:
                knockBackGauge = BaseWeapon.DEFAULT_KNOCKBACK_POWER * 3;
                break;
            case 3:
                knockBackGauge = BaseWeapon.DEFAULT_KNOCKBACK_POWER * 4;
                break;
        }

        Player.Instance.weaponManager.Weapon.KnockBackPower = knockBackGauge;
    }

    public override void DashAttack(BaseState state)
    {
        MultiplierDamage = 1.5f;
        VCam.Instance.SetImpulseOptions(gain: 0.75f, amplitude: 1f, frequency: 1, duration: 0.75f);
        Player.Instance.weaponManager.Weapon.KnockBackPower = BaseWeapon.DEFAULT_KNOCKBACK_POWER * 4;

        Player.Instance.animator.SetBool(hashDashAttackAnimation, true);
        if (dashAttackCoroutine != null)
            StopCoroutine(dashAttackCoroutine);
        dashAttackCoroutine = StartCoroutine(ProcessDashAttackPhysics(state));
    }

    public override void ChargingAttack(BaseState state)
    {
        MultiplierDamage = 1f;
        VCam.Instance.SetImpulseOptions(gain: 0.5f, amplitude: 1f, frequency: 1, duration: 0.75f);
        Player.Instance.weaponManager.Weapon.KnockBackPower = BaseWeapon.DEFAULT_KNOCKBACK_POWER * 4;
        Player.Instance.animator.SetBool(hashCharingAttackAnimation, true);
    }

    public override void Skill(BaseState state)
    {
        MultiplierDamage = 1f;
        VCam.Instance.SetImpulseOptions(gain: 1.5f, amplitude: 1f, frequency: 2, duration: 3f);
        Player.Instance.weaponManager.Weapon.KnockBackPower = BaseWeapon.DEFAULT_KNOCKBACK_POWER * 7;
        Player.Instance.animator.SetBool(hashSkillAnimation, true);
        Reporter.Report();
    }

    public override void UltimateSkill(BaseState state)
    {
        /*
         *   시간 관계 상 구현 범위에서 제외하도록 하였음
         */
    }

    private IEnumerator ProcessDashAttackPhysics(BaseState state)
    {
        DashAttackState _state = state as DashAttackState;
        Player.Instance.rigidBody.velocity = _state.direction * (Player.Instance.MoveSpeed * MoveState.CONVERT_UNIT_VALUE) * dashAttackPower;
        yield return dashAttackSecond;
        Player.Instance.rigidBody.velocity = Vector3.zero;
    }

    public void PlayComboAttackEffects()
    {
        int comboCount = Mathf.Clamp(ComboCount - 1, 0, 3);
        GameObject effect = Instantiate(defaultAttackEffs[comboCount]);
        Vector3 targetDirection = Player.Instance.Controller.MouseDirection;

        effect.transform.position = Player.Instance.effectGenerator.position;

        Vector3 secondAttackAdjustAngle = ComboCount == 2 ? new Vector3(0f, -90f, 0f) : Vector3.zero;
        effect.transform.rotation = Quaternion.LookRotation(targetDirection);
        effect.transform.eulerAngles += secondAttackAdjustAngle;
        effect.GetComponent<ParticleSystem>().Play();
    }

    public void PlayDashAttackEffect()
    {
        GameObject effect = Instantiate(dashAttackEffs);
        Vector3 targetDirection = Player.Instance.Controller.MouseDirection;

        effect.transform.position = Player.Instance.effectGenerator.position;
        effect.transform.rotation = Quaternion.LookRotation(targetDirection);
        effect.GetComponent<ParticleSystem>().Play();
    }

    public void PlayCharingEffect()
    {
        deleteChargingEffs = Instantiate(chargingEffs);
        deleteChargingEffs.transform.position = Player.Instance.weaponManager.Weapon.transform.position;
        deleteChargingEffs.GetComponent<ParticleSystem>().Play();
    }

    public void PlayChargingAttackEffect()
    {
        GameObject effect = Instantiate(chargingAttackEffs);
        effect.transform.position = Player.Instance.effectGenerator.position;
        effect.transform.rotation = Quaternion.LookRotation(Player.Instance.Controller.MouseDirection);
        effect.GetComponent<ParticleSystem>().Play();
    }

    public void DestroyEffect()
    {
        if(deleteChargingEffs != null)
        {
            Destroy(deleteChargingEffs);
        }
    }

    public void PlaySkillEffect()
    {
        GameObject effect = Instantiate(skillEffs);
        effect.transform.position = Player.Instance.effectGenerator.position;
        effect.transform.rotation = Quaternion.LookRotation(Player.Instance.transform.forward);
        effect.GetComponent<ParticleSystem>().Play();
    }

    public void PlayComboAttackSound()
    {
        var index = Mathf.Clamp(ComboCount - 1, 0, 3);
        Player.Instance.audioSource.PlayOneShot(attackSounds[index]);
    }

    public void PlayDashAttackSound()
    {
        Player.Instance.audioSource.PlayOneShot(attackSounds[2]);
    }

    public void PlayChargingSound()
    {
        Player.Instance.audioSource.PlayOneShot(chargingSound);
    }

    public void PlayChargingAttackSound()
    {
        Player.Instance.audioSource.PlayOneShot(chargingAttackSound);
    }

    public void PlaySkillSound()
    {
        Player.Instance.audioSource.PlayOneShot(skillSound);
    }

    public override void CalculateAttackDamage()
    {
        calculatedDamage = attackDamage * DataManager.CharonPaddleData[currentReinforceLevel].increasingAmount;
    }
}