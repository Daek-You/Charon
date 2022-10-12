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

        public readonly float dashPower;
        public readonly float dashTetanyTime;
        public readonly float dashCooltime;


        public DashState(PlayerController controller, float dashPower, float dashTetanyTime, float dashCoolTime) : base(controller)
        {
            inputDirectionBuffer = new Queue<Vector3>();
            this.dashPower = dashPower;
            this.dashTetanyTime = dashTetanyTime;
            this.dashCooltime = dashCoolTime;
            Hash_DashTrigger = Animator.StringToHash("Dash");
            Hash_IsDashBool = Animator.StringToHash("IsDashing");
            Hash_DashPlaySpeedFloat = Animator.StringToHash("DashPlaySpeed");   /// 이동 속도에 따라 대시 애니메이션 재생 속도를 달리 할 필요가 있음. 어떻게 할까?
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
            dashDirection = (dashDirection == Vector3.zero) ? Controller.transform.forward : dashDirection;

            Player.Instance.animator.SetBool(Hash_IsDashBool, true);
            Player.Instance.animator.SetTrigger(Hash_DashTrigger);
            Controller.LookAt(new Vector3(dashDirection.x, 0f, dashDirection.z));
            Player.Instance.rigidBody.velocity = dashDirection * (Player.Instance.MoveSpeed * MoveState.CONVERT_UNIT_VALUE) * dashPower;    // 이동 속도에 따라 유동적으로 달라질 수 있도록 하자.
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
