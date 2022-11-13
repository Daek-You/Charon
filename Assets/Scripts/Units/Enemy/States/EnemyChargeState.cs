using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargeState : CharacterController.BaseState
{
    private Enemy enemy;
    private float timer;
    public readonly int chargeAnimation;

    public EnemyChargeState(Enemy enemy)
    {
        this.enemy = enemy;
        chargeAnimation = Animator.StringToHash("Charge");
    }

    public override void OnEnterState()
    {
        enemy.agent.isStopped = true;
        enemy.rigidBody.isKinematic = true;
    }

    public override void OnExitState()
    {
        enemy.agent.isStopped = false;
        enemy.animator.SetBool(chargeAnimation, false);
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        timer += Time.deltaTime;
        if (timer > 2.0f)
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.Target.transform.position);
            if (distance > 5.0f)
            {
                enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_STIFFNESS);
                return;
            }
            else
            {
                enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_STIFFNESS);
                return;
            }
        }
        enemy.animator.SetBool(chargeAnimation, true);
    }
}
