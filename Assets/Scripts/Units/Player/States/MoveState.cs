using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterController
{
    public class MoveState : BaseState
    {
        public const float CONVERT_UNIT_VALUE = 0.01f;
        public const float DEFAULT_CONVERT_MOVESPEED = 3f;
        public const float DEFAULT_ANIMATION_PLAYSPEED = 0.9f;
        private int hashMoveAnimation;

        public MoveState(PlayerController controller) : base(controller)
        {
            hashMoveAnimation = Animator.StringToHash("Velocity");
        }

        protected float GetAnimationSyncWithMovement(float changedMoveSpeed)
        {
            if (Controller.inputDirection == Vector3.zero)
            {
                return -DEFAULT_ANIMATION_PLAYSPEED;
            }

            // (바뀐 이동 속도 - 기본 이동속도) * 0.1f
            return (changedMoveSpeed - DEFAULT_CONVERT_MOVESPEED) * 0.1f;
        }

        public override void OnEnterState()
        {

        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {
            float currentMoveSpeed = Controller.player.MoveSpeed * CONVERT_UNIT_VALUE;
            float animationPlaySpeed = DEFAULT_ANIMATION_PLAYSPEED + GetAnimationSyncWithMovement(currentMoveSpeed);
            Controller.LookAt(Controller.inputDirection);
            Controller.rigidBody.velocity = Controller.calculatedDirection * currentMoveSpeed + Controller.gravity;
            Controller.animator.SetFloat(hashMoveAnimation, animationPlaySpeed);
        }

        public override void OnExitState()
        {

        }
    }
}

