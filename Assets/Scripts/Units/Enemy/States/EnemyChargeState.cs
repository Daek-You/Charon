using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargeState : CharacterController.BaseState
{
    private Enemy enemy;
    private Char_Jinkwang bossEnemy;
    private float timer;
    public readonly int chargeAnimation;


    public EnemyChargeState(Enemy enemy)
    {
        this.enemy = enemy;
        if (enemy is Char_Jinkwang)
            bossEnemy = enemy as Char_Jinkwang;
        chargeAnimation = Animator.StringToHash("Charge");
    }

    public override void OnEnterState()
    {
        enemy.agent.isStopped = true;
        enemy.rigidBody.isKinematic = true;
        enemy.animator.SetBool(chargeAnimation, true);
        timer = bossEnemy.ChargeTimer;

    }

    public override void OnExitState()
    {
        enemy.agent.isStopped = false;
        enemy.animator.SetBool(chargeAnimation, false);
        bossEnemy.IsSecondAttack = false;
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        timer += Time.deltaTime;

        if (timer >= enemy.AttackDelay)
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.Target.transform.position);
            bossEnemy.ChargeTimer = 0f;

            if (distance <= enemy.AttackRange)
                enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_CLOSE_SKILL);
            else
                enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_FAR_SKILL);
        }
    }
}
