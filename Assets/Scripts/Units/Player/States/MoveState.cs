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

        public MoveState()
        {
            hashMoveAnimation = Animator.StringToHash("Velocity");
        }

        protected float GetAnimationSyncWithMovement(float changedMoveSpeed)
        {
            if (Player.Instance.Controller.inputDirection == Vector3.zero)
            {
                return -DEFAULT_ANIMATION_PLAYSPEED;
            }

            // (�ٲ� �̵� �ӵ� - �⺻ �̵��ӵ�) * 0.1f
            return (changedMoveSpeed - DEFAULT_CONVERT_MOVESPEED) * 0.1f;
        }

        public override void OnEnterState()
        {
            /// �������� �ʿ� X
        }

        public override void OnUpdateState()
        {
            /// �������� �ʿ� X
        }

        public override void OnFixedUpdateState()
        {
            float currentMoveSpeed = Player.Instance.MoveSpeed * CONVERT_UNIT_VALUE;
            float animationPlaySpeed = DEFAULT_ANIMATION_PLAYSPEED + GetAnimationSyncWithMovement(currentMoveSpeed);
            Player.Instance.Controller.LookAt(Player.Instance.Controller.inputDirection);
            Player.Instance.rigidBody.velocity = Player.Instance.Controller.calculatedDirection * currentMoveSpeed + Player.Instance.Controller.gravity;
            Player.Instance.animator.SetFloat(hashMoveAnimation, animationPlaySpeed);
        }

        public override void OnExitState()
        {
            Player.Instance.animator.SetFloat(hashMoveAnimation, 0f);
            Player.Instance.rigidBody.velocity = Vector3.zero;
        }
    }
}

