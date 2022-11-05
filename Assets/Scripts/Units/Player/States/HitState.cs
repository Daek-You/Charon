using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : CharacterController.BaseState
{
    public bool IsHit { get; set; }
    public const float HIT_TETANY_TIME = 0.7f;
    public readonly int hitAnimation = Animator.StringToHash("Hit");
    private float timer;

    public HitState()
    {

    }

    public override void OnEnterState()
    {
        IsHit = true;
        Player.Instance.animator.SetTrigger(hitAnimation);
        timer = 0;
    }

    public override void OnExitState()
    {
        IsHit = false;
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        timer += Time.deltaTime;

        if(timer >= HIT_TETANY_TIME)
        {
            Player.Instance.stateMachine.ChangeState(CharacterController.StateName.MOVE);
        }
    }
}
