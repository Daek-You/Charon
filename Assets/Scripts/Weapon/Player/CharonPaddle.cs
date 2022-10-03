using CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharonPaddle : BaseWeapon
{
    public readonly int hashIsAttackAnimation = Animator.StringToHash("IsAttack");
    public readonly int hashAttackAnimation = Animator.StringToHash("AttackCombo");
    public readonly int hashAttackSpeedAnimation = Animator.StringToHash("AttackSpeed");
    private Coroutine checkAttackReInputCor;

    public override void Attack(BaseState state)
    {
        ComboCount++;
        Player.Instance.animator.SetFloat(hashAttackSpeedAnimation, AttackSpeed);
        Player.Instance.animator.SetBool(hashIsAttackAnimation, true);
        Player.Instance.animator.SetInteger(hashAttackAnimation, ComboCount);
        CheckAttackReInput(AttackState.CanReInputTime);
    }

    public override void ChargingAttack(BaseState state)
    {

    }

    public override void DashAttack(BaseState state)
    {

    }

    public override void Skill(BaseState state)
    {

    }

    public override void UltimateSkill(BaseState state)
    {

    }

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
        Player.Instance.animator.SetInteger(hashAttackAnimation, 0);
    }
}