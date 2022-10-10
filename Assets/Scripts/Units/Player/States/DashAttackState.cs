using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class DashAttackState : BaseState
    {
        public static bool IsDashAttack = false;

        public DashAttackState(PlayerController controller) : base(controller) { }

        public override void OnEnterState()
        {
            Player.Instance.rigidBody.velocity = Vector3.zero;
            Player.Instance.animator.applyRootMotion = false;
            IsDashAttack = true;
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

