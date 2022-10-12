using CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    private Coroutine dashCoolTimeCoroutine;
    SkinnedMeshRenderer skinnedMeshRenderer;     /// 쿨타임 시각용 테스트 변수
    Color originColor;                           /// 쿨타임 시각용 테스트 변수

    private DashState dashState;
    private DashAttackState dashAttackState;

    void Start()
    {
        dashState = Player.Instance.stateMachine.GetState(StateName.DASH) as DashState;
        dashAttackState = Player.Instance.stateMachine.GetState(StateName.DASH_ATTACK) as DashAttackState;
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = skinnedMeshRenderer.material.color;
    }

    public void OnFinishedAttack()
    {
        AttackState.IsAttack = false;
        Player.Instance.animator.SetBool("IsAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedDashAttack()
    {
        dashAttackState.IsDashAttack = false;
        dashState.CanDashAttack = false;
        Player.Instance.animator.SetBool("IsDashAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnCanDashAttack()
    {
        dashState.CanDashAttack = true;
    }


    public void OnChangeDashToDashAttack()
    {
        if (dashAttackState.IsPressDashAttack)
        {
            dashAttackState.IsPressDashAttack = false;
            dashState.inputDirectionBuffer.Clear();
            Player.Instance.stateMachine.ChangeState(StateName.DASH_ATTACK);
            return;
        }
    }

    public void OnFinishedDash()
    {
        if (!dashAttackState.IsDashAttack)
        {
            dashState.CanDashAttack = false;

            if (dashState.inputDirectionBuffer.Count > 0)
            {
                Player.Instance.stateMachine.ChangeState(StateName.DASH);
                return;
            }

            Player.Instance.stateMachine.ChangeState(StateName.MOVE);

            if (dashCoolTimeCoroutine != null)
                StopCoroutine(dashCoolTimeCoroutine);
            dashCoolTimeCoroutine = StartCoroutine(CheckDashReInputLimitTime(dashState.dashCooltime));
        }
    }



    public void OnCanAddToDashInputBuffer()
    {
        dashState.CanAddInputBuffer = true;
    }

    private IEnumerator CheckDashReInputLimitTime(float limitTime)
    {
        float timer = 0f;
        skinnedMeshRenderer.material.color = Color.red;

        while (true)
        {
            timer += Time.deltaTime;
            
            if(timer > limitTime)
            {
                dashState.Reset();
                skinnedMeshRenderer.material.color = originColor;
                break;
            }
            yield return null;
        }
    }
}
