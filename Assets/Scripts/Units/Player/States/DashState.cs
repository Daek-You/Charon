using System.Collections;
using UnityEngine;


namespace CharacterController
{
    public class DashState : BaseState
    {
        #region #대시 관련 변수
        public static int CurrentDashCount { get; private set; } = 0;
        public float dashPower { get; private set; } = 2f;
        public float dashForwardRollTime { get; private set; } = 0.2f;
        public float dashReInputTime { get; private set; } = 0.2f;
        public float dashTetanyTime { get; private set; } = 0.1f;
        public float dashCoolTime { get; private set; } = 0.5f;
        private int hashDashAnimation;
        private int hashDashBoolAnimation;
        #endregion

        #region #대시 동작별 스위치 변수
        public static bool IsDash = false;
        public static bool CanOtherBehaviour { get; private set; } = false;
        private bool onReInputSwitch;
        private bool onTetanySwitch;
        private bool onSwitchDash;
        private bool isDashFinished;
        private float timer = 0f;
        #endregion

        public DashState(PlayerController controller) : base(controller)
        {
            hashDashAnimation = Animator.StringToHash("Dash");
            hashDashBoolAnimation = Animator.StringToHash("IsDashing");
        }

        public void SetDashSettings(float dashPower, float dashForwardRollTime, float dashReInputTime, float dashTetanyTime, float dashCoolTime)
        {
            this.dashPower = dashPower;
            this.dashForwardRollTime = dashForwardRollTime;
            this.dashReInputTime = dashReInputTime;
            this.dashTetanyTime = dashTetanyTime;
            this.dashCoolTime = dashCoolTime;
        }

        public override void OnEnterState()
        {
            IsDash = true;
            CurrentDashCount++;
            onSwitchDash = true;
            onReInputSwitch = true;
            onTetanySwitch = true;
            isDashFinished = false;
            CanOtherBehaviour = false;
        }

        public override void OnUpdateState()
        {
            if (!onSwitchDash && !isDashFinished)
            {
                timer += Time.deltaTime;
                float validReInputTime = dashForwardRollTime + dashReInputTime;
                float validTetanyTime = validReInputTime + dashTetanyTime;
                bool canDashReInput = (timer >= dashForwardRollTime) && (timer < validReInputTime);
                bool isTetanyTime = (timer >= validReInputTime) && (timer < validTetanyTime);
      

                if (canDashReInput && onReInputSwitch)      // 재입력이 가능한 시간대로 진입한 경우
                {
                    CanOtherBehaviour = true;
                    onReInputSwitch = false;
                }
                else if (isTetanyTime && onTetanySwitch)    // 대시 동작이 끝난 경우
                {
                    Player.Instance.animator.SetBool(hashDashBoolAnimation, false);
                    Player.Instance.rigidBody.velocity = Vector3.zero;
                    onTetanySwitch = false;
                    CanOtherBehaviour = false;
                }
                else if (!isTetanyTime && !onTetanySwitch)  // 대시 동작이 끝나고 경직 시간이 지나고 상태 전환
                {
                    isDashFinished = true;
                    Player.Instance.stateMachine.ChangeState(StateName.MOVE);
                }
            }
        }

        public override void OnFixedUpdateState()
        {
            if (onSwitchDash)
            {
                Player.Instance.animator.applyRootMotion = false;
                Vector3 LookAtDirection = (Controller.inputDirection == Vector3.zero) ? Controller.transform.forward : Controller.inputDirection;
                Vector3 dashDirection = (Controller.calculatedDirection == Vector3.zero) ? Controller.transform.forward : Controller.calculatedDirection;

                Player.Instance.animator.SetFloat("Velocity", 0f);
                Player.Instance.animator.SetBool(hashDashBoolAnimation, true);
                Player.Instance.animator.SetTrigger(hashDashAnimation);
                Controller.LookAt(LookAtDirection);
                Player.Instance.rigidBody.velocity = dashDirection * (Player.Instance.MoveSpeed * MoveState.CONVERT_UNIT_VALUE) * dashPower;    // 이동 속도에 따라 유동적으로 달라질 수 있도록 하자.
                onSwitchDash = false;
                timer = 0f;
            }
        }
   
        public override void OnExitState()
        {
            Player.Instance.animator.SetBool(hashDashBoolAnimation, false);
            CurrentDashCount = (CurrentDashCount >= Controller.player.DashCount) ? 0 : CurrentDashCount;
            timer = 0f;
            CanOtherBehaviour = false;
            IsDash = false;
            Player.Instance.animator.applyRootMotion = true;
        }
    }
}
