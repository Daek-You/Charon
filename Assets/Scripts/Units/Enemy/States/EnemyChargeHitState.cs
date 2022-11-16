using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChargeHitState : EnemyHitState
{
    private Char_Jinkwang bossEnemy;
    public readonly int chargeAnimation;

    public EnemyChargeHitState(Enemy enemy) : base(enemy)
    {
        this.enemy = enemy;
        if (enemy is Char_Jinkwang)
            bossEnemy = enemy as Char_Jinkwang;
        chargeAnimation = Animator.StringToHash("Charge");
    }

    public override void OnEnterState()
    {
        IsHit = true;
        timer = 0f;
        enemy.rigidBody.isKinematic = false;
        enemy.agent.isStopped = true;

        for (int i = 0; i < enemy.skinnedMeshRenderers.Length; i++)
        {
            enemy.skinnedMeshRenderers[i].material.color = Color.red;
        }

        enemy.animator.SetBool(chargeAnimation, true);
    }

    public override void OnExitState()
    {
        enemy.rigidBody.isKinematic = true;
        enemy.agent.isStopped = false;
        IsHit = false;

        for (int i = 0; i < enemy.skinnedMeshRenderers.Length; i++)
        {
            enemy.skinnedMeshRenderers[i].material.color = enemy.originColors[i];
        }

        enemy.rigidBody.velocity = Vector3.zero;
        bossEnemy.ChargeTimer += timer;
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        bossEnemy.ChargeTimer += Time.deltaTime;
        timer += Time.deltaTime;

        if (timer >= 0.25f && IsHit)
        {
            IsHit = false;
            enemy.rigidBody.velocity = Vector3.zero;

            if (bossEnemy.ChargeTimer > enemy.AttackDelay)
                enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_CHARGE);

        }
        else if (timer >= enemy.TetanyTime)
        {
            enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_CHARGE);
        }
    }
}
