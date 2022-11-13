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
        for(int i =0; i< enemy.skinnedMeshRenderers.Length; i++)
        {
            enemy.skinnedMeshRenderers[i].material.color = Color.black;
        }

        enemy.animator.SetTrigger(dieAnimation);
        enemy.audioSource.PlayOneShot(enemy.effectSounds[Enemy.SoundType.DIE]);
        timer = 0f;
        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.DieEnemy, null);
    }

    public override void OnExitState()
    {
        for (int i = 0; i < enemy.skinnedMeshRenderers.Length; i++)
        {
            enemy.skinnedMeshRenderers[i].material.color = enemy.originColors[i];
        }
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
