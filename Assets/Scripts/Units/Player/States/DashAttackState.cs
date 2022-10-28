using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class DashAttackState : BaseState
    {
        public bool IsDashAttack { get; set; }
        public bool IsPressDashAttack { get; set; }
        public Vector3 direction { get; set; }

        public DashAttackState() { }

        public override void OnEnterState()
        {
            IsDashAttack = true;
            DashState dashState = Player.Instance.stateMachine.GetState(StateName.DASH) as DashState;
            dashState.IsDash = false;
            dashState.CanAddInputBuffer = false;
            dashState.CurrentDashCount = 0;

            Player.Instance.Controller.LookAt(direction);
            Player.Instance.rigidBody.velocity = Vector3.zero;
            Player.Instance.animator.applyRootMotion = false;
            Player.Instance.weaponManager.Weapon?.DashAttack(this);
        }

        public override void OnExitState()
        {
            Player.Instance.animator.applyRootMotion = true;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
        }
    }
}

