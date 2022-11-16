using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : CharacterController.BaseState
{
    public bool isAttack { get; set; }
    public bool isCheckedPlayerPosition { get; set; }
    private Enemy enemy;
    public Quaternion targetAngle { get; private set; }
    private float timer = 0f;
    private const float ROTATE_TIME = 0.5f;

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
            timer = 0f;
            return;
        }

        if (Quaternion.Angle(enemy.transform.rotation, targetAngle) > 5f && !isAttack && timer < ROTATE_TIME)
        {
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetAngle, enemy.RotationSpeed * 2 * Time.deltaTime);
            timer += enemy.RotationSpeed * 1.5f * Time.deltaTime;
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
