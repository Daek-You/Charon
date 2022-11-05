using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : CharacterController.BaseState
{
    public bool isAttack { get; set; }
    private Enemy enemy;

    public EnemyAttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void OnEnterState()
    {
        isAttack = true;
        enemy.Weapon?.Attack();
    }

    public override void OnExitState()
    {
        enemy.Weapon?.StopAttack();
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        bool isOverRange = Vector3.Distance(enemy.transform.position, Player.Instance.transform.position) > enemy.agent.stoppingDistance;
        if (isOverRange && !isAttack)
        {
            enemy.Weapon?.StopAttack();
            enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_MOVE);
        }

        else if (!isAttack)
        {
            isAttack = true;
            enemy.Weapon?.Attack();
        }
    }
}
