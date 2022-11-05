using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHitState : CharacterController.BaseState
{
    public bool IsHit { get; set; }
    private readonly int damagedAnimation = Animator.StringToHash("Damaged");
    private SkinnedMeshRenderer renderer;
    private Color originalColor;
    private Enemy enemy;
    private float timer;

    public EnemyHitState(Enemy enemy)
    {
        this.enemy = enemy;
        renderer = enemy.GetComponentInChildren<SkinnedMeshRenderer>();
        originalColor = renderer.material.color;
    }

    public override void OnEnterState()
    {
        enemy.animator.SetTrigger(damagedAnimation);
        IsHit = true;
        timer = 0f;
        enemy.rigidBody.isKinematic = false;
        enemy.agent.isStopped = true;

        Vector3 direction = (enemy.transform.position - Player.Instance.transform.position).normalized;

        var knockBackPower = Player.Instance.weaponManager.Weapon.KnockBackPower;


        enemy.rigidBody.AddForce(direction * knockBackPower, ForceMode.Impulse);
        renderer.material.color = Color.red;
    }

    public override void OnExitState()
    {
        renderer.material.color = originalColor;
        enemy.rigidBody.velocity = Vector3.zero;
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        timer += Time.deltaTime;

        if (timer >= 0.25f && IsHit)
        {
            IsHit = false;
            enemy.rigidBody.velocity = Vector3.zero;
        }

        else if (timer >= Enemy.HIT_TIME)
        {
            enemy.stateMachine.ChangeState(CharacterController.StateName.ENEMY_MOVE);
        }
    }
}
