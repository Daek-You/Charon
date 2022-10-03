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
}
