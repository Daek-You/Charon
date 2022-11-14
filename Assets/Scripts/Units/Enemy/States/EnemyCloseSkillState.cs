using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCloseSkillState : CharacterController.BaseState
{
    private Enemy enemy;
    public readonly int skillAnimation;
    public bool IsAttack { get; private set; }

    public EnemyCloseSkillState(Enemy enemy)
    {
        this.enemy = enemy;
        skillAnimation = Animator.StringToHash("Close");
    }

    public override void OnEnterState()
    {
        IsAttack = true;
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
        if (!IsAttack)
        {
            enemy.Weapon?.StopAttack();
            enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_STIFFNESS);
        }
    }
}
