using System.Collections;
using UnityEngine;


namespace CharacterController
{
    public class DashState : BaseState
    {
        public static int currentDashCount { get; private set; } = 0;
        public float dashPower { get; private set; } = 8f;
        public float dashForwardRollTime { get; private set; } = 0.2f;
        public float dashReInputTime { get; private set; } = 0.2f;
        public float dashTetanyTime { get; private set; } = 0.1f;
        public float dashCoolTime { get; private set; } = 0.5f;

        private WaitForSeconds DASH_FORWARD_ROLL_TIME;
        private WaitForSeconds DASH_RE_INPUT_TIME;
        private WaitForSeconds DASH_TETANY_TIME;
        private Coroutine dashCoroutine;
        private Coroutine dashCoolTimeCoroutine;
        private int hashDashAnimation;


        public void SetDashSettings(float dashPower, float dashForwardRollTime, float dashReInputTime, float dashTetanyTime, float dashCoolTime)
        {
            this.dashPower = dashPower;
            this.dashForwardRollTime = dashForwardRollTime;
            this.dashReInputTime = dashReInputTime;
            this.dashTetanyTime = dashTetanyTime;
            this.dashCoolTime = dashCoolTime;
        }

        public DashState(PlayerController controller) : base(controller)
        {
            DASH_FORWARD_ROLL_TIME = new WaitForSeconds(dashForwardRollTime);
            DASH_RE_INPUT_TIME = new WaitForSeconds(dashReInputTime);
            DASH_TETANY_TIME = new WaitForSeconds(dashTetanyTime);
        }

        public override void OnEnterState()
        {
            currentDashCount++;
            if (dashCoroutine != null && dashCoolTimeCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
                StopCoroutine(dashCoolTimeCoroutine);
            }

            dashCoroutine = StartCoroutine(DashCoroutine());
        }


        private IEnumerator DashCoroutine()
        {
            Vector3 LookAtDirection = (controller.inputDirection == Vector3.zero) ? transform.forward : controller.inputDirection;
            Vector3 dashDirection = (controller.calculatedDirection == Vector3.zero) ? transform.forward : controller.calculatedDirection;

            controller.animator.SetFloat("Velocity", 0f);
            controller.animator.SetBool("IsDashing", true);
            controller.animator.SetTrigger("Dash");
            controller.LookAt(LookAtDirection);
            controller.rigidBody.velocity = dashDirection * dashPower;

            yield return DASH_FORWARD_ROLL_TIME;       // 대시 앞구르기 모션 시간
            bool canNDash = controller.player.DashCount > 1 && currentDashCount < controller.player.DashCount;
           
            if (canNDash)
            {
                //controller.stateMachine.ChangeState()
            }

            yield return DASH_RE_INPUT_TIME;           // N단 대시가 가능할 때, 키 입력을 받을 수 있는 시간
            controller.animator.SetBool("IsDashing", false);
            controller.rigidBody.velocity = Vector3.zero;

            yield return DASH_TETANY_TIME;             // 대시 후, 경직 시간
            controller.stateMachine.ChangeState(StateName.MOVE);
            dashCoolTimeCoroutine = StartCoroutine(DashCoolTimeCoroutine());   // 대시 쿨타임 체크 시작
        }

        private IEnumerator DashCoolTimeCoroutine()
        {
            float currentTime = 0f;
            while (true)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= dashCoolTime)
                    break;
                yield return null;
            }

            if (currentDashCount == controller.player.DashCount)
                currentDashCount = 0;
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
