using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class ChargingAttackState : BaseState
    {
        public bool IsChargingAttack { get; set; }

        public override void OnEnterState()
        {
            IsChargingAttack = true;
            Player.Instance.weaponManager.Weapon.ChargingAttack(this);
        }

        public override void OnExitState()
        {
            IsChargingAttack = false;
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
        }
    }
}

