using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : CharacterController.BaseState
{
    public bool isAttack { get; set; }
    public bool isCheckedPlayerPosition { get; set; }
    private Enemy enemy;
    public Quaternion targetAngle { get; private set; }

    public EnemyAttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public override void OnEnterState()
    {
        isAttack = false;
        Vector3 direction = (enemy.Target.position - enemy.transform.position).normalized;
        targetAngle = Quaternion.LookRotation(direction);
        isCheckedPlayerPosition = false;

        //isAttack = true;
        //Vector3 direction = (Player.Instance.transform.position - enemy.transform.position).normalized;
        //enemy.LookAt(direction);
        //enemy.Weapon?.Attack();
    }

    public override void OnExitState()
    {
        isAttack = false;
        isCheckedPlayerPosition = false;
        enemy.isCooltimeDone = true;
        enemy.Weapon?.StopAttack();
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        bool isOverRange = Vector3.Distance(enemy.transform.position, enemy.Target.position) > enemy.AttackRange;
        Vector3 direction = (enemy.Target.position - enemy.transform.position).normalized;

        if (isOverRange && !isAttack)
        {
            enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_MOVE);
            return;
        }

        if (!isAttack && !isCheckedPlayerPosition)
        {
            isCheckedPlayerPosition = true;
            targetAngle = Quaternion.LookRotation(direction);
            return;
        }

        if (Quaternion.Angle(enemy.transform.rotation, targetAngle) > 5f && !isAttack)
        {
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetAngle, enemy.RotationSpeed * 2 * Time.deltaTime);
            return;
        }

        if (!isAttack)
        {
            isAttack = true;
            enemy.isCooltimeDone = false;
            enemy.transform.rotation = targetAngle;
            enemy.Weapon?.Attack();
        }
    }
}
