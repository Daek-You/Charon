using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStayState : CharacterController.BaseState
{
    private Enemy enemy;
    public const float DEFAULT_ANIMATION_PLAYSPEED = 1f;
    private float timer;

    public EnemyStayState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void OnEnterState()
    {
        enemy.rigidBody.isKinematic = true;
        enemy.agent.isStopped = true;
    }

    public override void OnExitState()
    {

    }

    public override void OnFixedUpdateState()
    {

    }

    public override void OnUpdateState()
    {
        timer += Time.deltaTime;

        if (timer > 1.0f)
            enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_CHARGE);
    }
}
