using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargeState : CharacterController.BaseState
{
    private Enemy enemy;
    private Char_Jinkwang bossEnemy;
    private float timer;
    public readonly int chargeAnimation;
    private float CHARGE_TIME = 2.0f;
    private float ATTACK_DISTANCE = 8.0f;

    public EnemyChargeState(Enemy enemy)
    {
        this.enemy = enemy;
        if (enemy is Char_Jinkwang)
            bossEnemy = (Char_Jinkwang)enemy;
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
        bossEnemy.IsSecondAttackDone = false;

        if (timer < CHARGE_TIME)
            bossEnemy.ChargeTimer = timer;
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        timer += Time.deltaTime;
        if (timer >= CHARGE_TIME)
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.Target.transform.position);
            bossEnemy.ChargeTimer = 0.0f;

            if (distance < ATTACK_DISTANCE)
                enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_CLOSE_SKILL);
            else
                enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_FAR_SKILL);
        }
    }
}
