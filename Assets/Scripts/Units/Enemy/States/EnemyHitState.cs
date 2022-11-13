using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHitState : CharacterController.BaseState
{
    public bool IsHit { get; set; }
    private readonly int damagedAnimation = Animator.StringToHash("Damaged");
    private Enemy enemy;
    private float timer;

    public EnemyHitState(Enemy enemy)
    {
        this.enemy = enemy;
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

        for(int i =0; i< enemy.skinnedMeshRenderers.Length; i++)
        {
            enemy.skinnedMeshRenderers[i].material.color = Color.red;
        }
    }

    public override void OnExitState()
    {
        enemy.rigidBody.isKinematic = true;
        enemy.agent.isStopped = false;

        for (int i = 0; i < enemy.skinnedMeshRenderers.Length; i++)
        {
            enemy.skinnedMeshRenderers[i].material.color = enemy.originColors[i];
        }

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
