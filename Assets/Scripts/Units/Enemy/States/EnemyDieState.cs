using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : CharacterController.BaseState
{
    public readonly int dieAnimation = Animator.StringToHash("doDie");
    private Enemy enemy;
    private const float DISABLE_TIME = 5f;
    private float timer;

    public EnemyDieState(Enemy enemy)
    {
        this.enemy = enemy;   
    }
    
    public override void OnEnterState()
    {
        enemy.skinnedMeshRenderer.material.color = Color.black;
        enemy.animator.SetTrigger(dieAnimation);
        timer = 0f;
    }

    public override void OnExitState()
    {
        enemy.skinnedMeshRenderer.material.color = enemy.originMaterial.color;
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        timer += Time.deltaTime;

        if (timer >= DISABLE_TIME)
            enemy.gameObject.SetActive(false);
    }
}
