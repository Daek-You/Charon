using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterController
{
    public class DashState : BaseState
    {
        public int CurrentDashCount { get; set; } = 0;
        public bool CanAddInputBuffer { get; set; }
        public bool CanDashAttack { get; set; }
        public bool IsDash { get; set; }
        public int Hash_DashTrigger { get; private set; }
        public int Hash_IsDashBool { get; private set; }
        public int Hash_DashPlaySpeedFloat { get; private set; }
        public Queue<Vector3> inputDirectionBuffer { get; private set; }

        public const float DEFAULT_ANIMATION_SPEED = 2f;
        public readonly float dashPower;
        public readonly float dashTetanyTime;
        public readonly float dashCooltime;

        public DashState(float dashPower, float dashTetanyTime, float dashCoolTime)
        {
            inputDirectionBuffer = new Queue<Vector3>();
            this.dashPower = dashPower;
            this.dashTetanyTime = dashTetanyTime;
            this.dashCooltime = dashCoolTime;
            Hash_DashTrigger = Animator.StringToHash("Dash");
            Hash_IsDashBool = Animator.StringToHash("IsDashing");
            Hash_DashPlaySpeedFloat = Animator.StringToHash("DashPlaySpeed"); 
        }

        public override void OnEnterState()
        {
            IsDash = true;
            CanAddInputBuffer = false;
            CanDashAttack = false;
            Player.Instance.animator.applyRootMotion = false;
            Dash();
        }

        private void Dash()
        {
            Vector3 dashDirection = inputDirectionBuffer.Dequeue();
            dashDirection = (dashDirection == Vector3.zero) ? Player.Instance.Controller.transform.forward : dashDirection;

            Player.Instance.animator.SetBool(Hash_IsDashBool, true);
            Player.Instance.animator.SetTrigger(Hash_DashTrigger);
            Player.Instance.Controller.LookAt(new Vector3(dashDirection.x, 0f, dashDirection.z));

            float dashAnimationPlaySpeed = DEFAULT_ANIMATION_SPEED + (Player.Instance.MoveSpeed * MoveState.CONVERT_UNIT_VALUE - MoveState.DEFAULT_CONVERT_MOVESPEED) * 0.1f;
            Player.Instance.animator.SetFloat(Hash_DashPlaySpeedFloat, dashAnimationPlaySpeed);
            Player.Instance.rigidBody.velocity = dashDirection * (Player.Instance.MoveSpeed * MoveState.CONVERT_UNIT_VALUE) * dashPower;
        }

        public override void OnUpdateState()
        {

        }

        public override void OnFixedUpdateState()
        {

        }

        public override void OnExitState()
        {
            Player.Instance.rigidBody.velocity = Vector3.zero;
            Player.Instance.animator.applyRootMotion = true;
            Player.Instance.animator.SetBool(Hash_IsDashBool, false);
        }

        public void Reset()
        {
            IsDash = false;
            CanAddInputBuffer = false;
            CurrentDashCount = 0;
        }
    }
}
