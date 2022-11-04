using CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharonPaddle : BaseWeapon, IEffect
{
    public GameObject[] defaultAttackEffs;
    public GameObject dashAttackEffs;
    public GameObject chargingEffs;
    public GameObject chargingAttackEffs;
    public GameObject skillEffs;
    public GameObject deleteChargingEffs { get; set; }      // 차징 중 다른 상태로 넘어갈 때 제거

    public readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public readonly int hashAttackAnimation = Animator.StringToHash("AttackCombo");
    public readonly int hashAttackSpeedAnimation = Animator.StringToHash("AttackSpeed");
    public readonly int hashDashAttackAnimation = Animator.StringToHash("IsDashAttack");
    public readonly int hashCharingAttackAnimation = Animator.StringToHash("IsCharingAttack");
    public readonly int hashSkillAnimation = Animator.StringToHash("IsSkill");
    public const float dashAttackPower = 4f;
    private Coroutine dashAttackCoroutine;
    private WaitForSeconds dashAttackSecond = new WaitForSeconds(0.2f);

    public const float SkillCoolTime = 5f;

    public override void Attack(BaseState state)
    {
        ComboCount++;
        Player.Instance.animator.SetFloat(hashAttackSpeedAnimation, attackSpeed);
        Player.Instance.animator.SetBool(hashIsAttackAnimation, true);
        Player.Instance.animator.SetInteger(hashAttackAnimation, ComboCount);
        CheckAttackReInput(AttackState.CanReInputTime);
    }

    public override void DashAttack(BaseState state)
    {
        Player.Instance.animator.SetBool(hashDashAttackAnimation, true);
        if (dashAttackCoroutine != null)
            StopCoroutine(dashAttackCoroutine);
        dashAttackCoroutine = StartCoroutine(ProcessDashAttackPhysics(state));
    }

    public override void ChargingAttack(BaseState state)
    {
        Player.Instance.animator.SetBool(hashCharingAttackAnimation, true);
    }

    public override void Skill(BaseState state)
    {
        Player.Instance.animator.SetBool(hashSkillAnimation, true);
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
}