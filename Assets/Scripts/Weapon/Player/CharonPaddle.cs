using CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharonPaddle : BaseWeapon
{
    public readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public readonly int hashAttackAnimation = Animator.StringToHash("AttackCombo");
    public readonly int hashAttackSpeedAnimation = Animator.StringToHash("AttackSpeed");
    public readonly int hashDashAttackAnimation = Animator.StringToHash("IsDashAttack");
    private Coroutine checkAttackReInputCor;
    private Coroutine dashAttackCoroutine;
    private WaitForSeconds dashAttackSecond = new WaitForSeconds(0.2f);

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
    }


    public override void Skill(BaseState state)
    {

    }

    public override void UltimateSkill(BaseState state)
    {

    }
    private IEnumerator ProcessDashAttackPhysics(BaseState state)
    {
        Player.Instance.rigidBody.velocity = state.Controller.MouseDirection * (Player.Instance.MoveSpeed * MoveState.CONVERT_UNIT_VALUE) * 3;
        yield return dashAttackSecond;
        Player.Instance.rigidBody.velocity = Vector3.zero;
    }
}