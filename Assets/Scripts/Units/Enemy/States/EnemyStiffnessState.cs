using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStiffnessState : CharacterController.BaseState
{
    private Enemy enemy;
    public readonly int stiffAnimation;
    float timer;

    public EnemyStiffnessState(Enemy enemy)
    {
        this.enemy = enemy;
        stiffAnimation = Animator.StringToHash("Stiff");
    }

    public override void OnEnterState()
    {
        enemy.agent.isStopped = true;
        enemy.rigidBody.isKinematic = true;
    }

    public override void OnExitState()
    {
        enemy.agent.isStopped = false;
        enemy.animator.SetBool(stiffAnimation, false);
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
        enemy.animator.SetBool(stiffAnimation, true);
    }
}
