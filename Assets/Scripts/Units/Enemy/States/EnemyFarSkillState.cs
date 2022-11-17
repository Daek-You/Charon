using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFarSkillState : CharacterController.BaseState
{
    private Enemy enemy;
    //private Char_Jinkwang bossEnemy;
    public bool IsAttack { get; set; }
    public bool isCheckedPlayerPosition { get; set; }
    public Quaternion targetAngle { get; private set; }
    public readonly int AttackAnimation;
    private float timer = 0f;
    private const float ROTATE_TIME = 0.5f;

    public EnemyFarSkillState(Enemy enemy)
    {
        this.enemy = enemy;
        //bossEnemy = enemy as Char_Jinkwang;
        AttackAnimation = Animator.StringToHash("IsFarAttack");
    }

    public override void OnEnterState()
    {
        enemy.Weapon.AttackDamage = enemy.Weapon.originalDamage * 1.5f;
        VCam.Instance.SetImpulseOptions(gain: 0.6f, amplitude: 0.65f, frequency: 1, duration: 0.6f);

        IsAttack = false;
        Vector3 direction = (enemy.Target.position - enemy.transform.position).normalized;
        targetAngle = Quaternion.LookRotation(direction);
        isCheckedPlayerPosition = false;
        enemy.rigidBody.isKinematic = false;

        var weapon = enemy.Weapon as Char_Jinkwang_Weapon;
        if (weapon != null)
            weapon.AttackAnimation = AttackAnimation;
    }

    public override void OnExitState()
    {
        enemy.Weapon.AttackDamage = enemy.Weapon.originalDamage;
        enemy.animator.applyRootMotion = true;
        IsAttack = false;
        isCheckedPlayerPosition = false;
        enemy.isCooltimeDone = true;
        enemy.rigidBody.isKinematic = true;
        enemy.Weapon?.StopAttack();
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        Vector3 direction = (enemy.Target.position - enemy.transform.position).normalized;

        if (!IsAttack && !isCheckedPlayerPosition)
        {
            isCheckedPlayerPosition = true;
            targetAngle = Quaternion.LookRotation(direction);
            timer = 0f;
            return;
        }

        if (Quaternion.Angle(enemy.transform.rotation, targetAngle) > 5f && !IsAttack && timer < ROTATE_TIME)
        {
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetAngle, enemy.RotationSpeed * 2 * Time.deltaTime);
            timer += enemy.RotationSpeed * 1.5f * Time.deltaTime;
            return;
        }

        if (!IsAttack)
        {
            IsAttack = true;
            enemy.isCooltimeDone = false;
            enemy.transform.rotation = targetAngle;

            //if (bossEnemy != null)
            //    bossEnemy.Direction = direction;

            enemy.Weapon?.Attack();
        }
    }
}
