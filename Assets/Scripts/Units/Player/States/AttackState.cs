using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AttackState : BaseState
    {
        public bool IsAttack { get; set; } = false;
        public const float CanReInputTime = 1f;

        public AttackState(PlayerController controller) : base(controller) { }

        public override void OnEnterState()
        {
            IsAttack = true;
            Player.Instance.weaponManager.Weapon?.Attack(this);
        }

        public override void OnExitState()
        {
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
        }
    }
}
