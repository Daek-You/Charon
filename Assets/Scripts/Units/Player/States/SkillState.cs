using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class SkillState : BaseState
    {
        public bool IsSkillActive { get; set; }

        public override void OnEnterState()
        {
            IsSkillActive = true;
            Player.Instance.weaponManager.Weapon.Skill(this);
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

