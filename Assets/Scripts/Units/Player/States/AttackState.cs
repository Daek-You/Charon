using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class AttackState : BaseState
    {
        public static int ComboCount { get; private set; } = 0;
        public static bool canAttack { get; private set; }
        public const float ValidReInputTime = 0.5f;
        public AttackState(PlayerController controller) : base(controller) { }
        private float timer = 0f;
        private bool onSwitchTimer;

        public override void OnEnterState()
        {
            ComboCount++;
            Controller.animator.SetInteger("AttackCombo", ComboCount);
            timer = 0f;
            onSwitchTimer = true;
            canAttack = true;
            //Controller.player.weaponManager.Weapon.Attack(Controller.MouseDirection);
        }

        public override void OnExitState()
        {
            onSwitchTimer = false;
            canAttack = false;
            if(ComboCount >= 3)
            {
                ComboCount = 0;
                Controller.animator.SetInteger("AttackCombo", 0);
            }
        }

        public override void OnFixedUpdateState()
        {
        }

        public override void OnUpdateState()
        {
            if (onSwitchTimer)
            {
                timer += Time.deltaTime;

                if(timer >= ValidReInputTime)
                {
                    Controller.stateMachine.ChangeState(StateName.MOVE);
                }
            }
        }
    }
}

