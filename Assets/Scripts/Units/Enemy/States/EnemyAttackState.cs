using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : CharacterController.BaseState
{

    public bool isAttack { get; set; }
    public readonly int attackAnimation = Animator.StringToHash("IsAttack");
    private Enemy enemy;

    public EnemyAttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void OnEnterState()
    {
        isAttack = false;
    }

    public override void OnExitState()
    {
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        if (!enemy.isAlived && !isAttack)
        {
            enemy.animator.SetBool(attackAnimation, false);
            enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_MOVE);
            return;
        }

        else if (!isAttack)
        {
            isAttack = true;
            enemy.animator.SetBool(attackAnimation, true);
        }
    }
}
