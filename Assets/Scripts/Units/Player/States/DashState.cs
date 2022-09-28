using System.Collections;
using UnityEngine;


namespace CharacterController
{
    public class DashState : BaseState
    {
        #region #��� ���� ����
        public static int CurrentDashCount { get; private set; } = 0;
        public float dashPower { get; private set; } = 8f;
        public float dashForwardRollTime { get; private set; } = 0.2f;
        public float dashReInputTime { get; private set; } = 0.2f;
        public float dashTetanyTime { get; private set; } = 0.1f;
        public float dashCoolTime { get; private set; } = 0.5f;
        private int hashDashAnimation;
        private int hashDashBoolAnimation;
        #endregion

        #region #��� ���ۺ� ����ġ ����
        public static bool CanOtherBehaviour { get; private set; } = true;
        private bool onReInputSwitch;
        private bool onTetanySwitch;
        private bool onSwitchDash;
        private bool isDashFinished;
        private float timer = 0f;
        #endregion

        public DashState(PlayerController Controller) : base(Controller)
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
      

                if (canDashReInput && onReInputSwitch)      // ���Է��� ������ �ð���� ������ ���
                {
                    CanOtherBehaviour = true;
                    onReInputSwitch = false;
                }
                else if (isTetanyTime && onTetanySwitch)    // ��� ������ ���� ���
                {
                    Controller.animator.SetBool(hashDashBoolAnimation, false);
                    Controller.rigidBody.velocity = Vector3.zero;
                    onTetanySwitch = false;
                    CanOtherBehaviour = false;
                }
                else if (!isTetanyTime && !onTetanySwitch)  // ��� ������ ������ ���� �ð��� ������ ���� ��ȯ
                {
                    isDashFinished = true;
                    Controller.stateMachine.ChangeState(StateName.MOVE);
                }
            }
        }

        public override void OnFixedUpdateState()
        {
            if (onSwitchDash)
            {
                Vector3 LookAtDirection = (Controller.inputDirection == Vector3.zero) ? Controller.transform.forward : Controller.inputDirection;
                Vector3 dashDirection = (Controller.calculatedDirection == Vector3.zero) ? Controller.transform.forward : Controller.calculatedDirection;

                Controller.animator.SetFloat("Velocity", 0f);
                Controller.animator.SetBool(hashDashBoolAnimation, true);
                Controller.animator.SetTrigger(hashDashAnimation);
                Controller.LookAt(LookAtDirection);
                Controller.rigidBody.velocity = dashDirection * dashPower;
                onSwitchDash = false;
                timer = 0f;
            }
        }
   
        public override void OnExitState()
        {
            CurrentDashCount = (CurrentDashCount >= Controller.player.DashCount) ? 0 : CurrentDashCount;
            timer = 0f;
            CanOtherBehaviour = true;
        }
    }
}
