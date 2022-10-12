using CharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SondolAnimationEvents : MonoBehaviour
{
    public void OnFinishedAttack()
    {
        AttackState.IsAttack = false;
        Player.Instance.animator.SetBool("IsAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnFinishedDashAttack()
    {
        DashAttackState.IsDashAttack = false;
        Player.Instance.animator.SetBool("IsDashAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
        Debug.Log("상태전환 : DashAttack -> Move");
    }

    public void OnFinishedDash()
    {

    }
}
