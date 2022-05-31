using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Move_ : ICommand<Controller>
{
    public static bool enabled = true;
    public static bool hasControl { get; private set; }
    public static Vector3 velocity { get; private set; }
    private int moveAnimation = Animator.StringToHash("Move");
    private float animationPlaySpeed = 0.9f;


    public void InputUpdate(Controller owner)
    {
        if (enabled)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            bool hasHorizontal = !Mathf.Approximately(horizontal, 0f);
            bool hasVertical = !Mathf.Approximately(vertical, 0f);
            hasControl = hasHorizontal || hasVertical;

            velocity = hasControl ? new Vector3(horizontal, 0f, vertical).normalized : Vector3.zero;

            // 나중에 이동 속도에 맞게 배속을 결정하는 공식 계산 필요
            float moveValue = hasControl ? animationPlaySpeed : 0f;
            owner.animator.SetFloat(moveAnimation, moveValue);
        }

        else if (hasControl && !enabled)    // 이전 움직임이 있고 비활성화가 된 경우
        {
            hasControl = false;
            velocity = Vector3.zero;
            owner.animator.SetFloat(moveAnimation, 0f);
        }

    }

    public void PhysicsUpdate(Controller owner)
    {
        if (enabled && !Dash.Lock)
        {
            owner.Move();
        }
    }
}




public class Dash : ICommand<Controller>
{
    public static bool hasInput = false;
    public static bool IsTimeOver = false;
    public static bool IsDash = false;
    public static bool Lock = false;
    public static Vector3 dashVector;
    public static int currentDashCount = 0;
    private const float dashPower = 10f;
    private const float conditionalTime = 0.0001f;
    private const float dashDuration = 0.55f;
    private int dashAnimation = Animator.StringToHash("Dash");


    public void InputUpdate(Controller owner)
    {

        hasInput = Input.GetKeyDown(KeyCode.Space) && currentDashCount < owner.sondol.dashCount;

        if (hasInput)
        {
            //owner.TimeCheck(Time.time, conditionalTime);

            if (!IsDash)
            {
                Lock = true;
                IsDash = true;
                dashVector = Move_.velocity;
                owner.DashMove(dashAnimation, dashPower, conditionalTime, dashDuration);
                ++currentDashCount;
                Debug.Log($"{currentDashCount}");
            }

        }
    }

    public void PhysicsUpdate(Controller owner)
    {
    }
}

