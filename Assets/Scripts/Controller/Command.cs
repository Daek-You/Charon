using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICommand<T>
{
    void InputUpdate(T owner);
    void PhysicsUpdate(T owner);
}


public class Move_ : ICommand<Sondol>
{
    public static bool enabled = true;
    private Vector3 direction;
    private int moveAnimation = Animator.StringToHash("Move");
    private float animationPlaySpeed = 0.9f;


    public void InputUpdate(Sondol owner)
    {
        if (enabled)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            
            bool hasHorizontal = !Mathf.Approximately(horizontal, 0f);
            bool hasVertical = !Mathf.Approximately(vertical, 0f);
            bool hasControl = hasHorizontal || hasVertical;

            direction = hasControl ? new Vector3(horizontal, 0f, vertical).normalized : Vector3.zero;
            
            // 나중에 이동 속도에 맞게 배속을 결정하는 공식 계산 필요
            float moveValue = hasControl ? animationPlaySpeed : 0f;
            owner.animator.SetFloat(moveAnimation, moveValue);
        }
    }

    public void PhysicsUpdate(Sondol owner)
    {
        owner.Move(direction);
    }
}




public class Dash : ICommand<Sondol>
{
    public static int currentDashCount = 0;
    private const float dashPower = 50f;
    private const float dashDelay = 0.5f;
    private const float dashStopTime = 0.25f;
    private int dashAnimation = Animator.StringToHash("Dash");

    public void InputUpdate(Sondol owner)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Move_.enabled = false;
            ++currentDashCount;
            owner.animator.SetTrigger(dashAnimation);
            //owner._Dash(dashPower, dashDelay, dashStopTime);
            Move_.enabled = true;
        }
    }

    public void PhysicsUpdate(Sondol owner) { }
}
