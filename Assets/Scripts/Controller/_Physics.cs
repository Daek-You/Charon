using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class _Physics : MonoBehaviour, IComponent<Controller>
{

    public float dashPower;

    private Rigidbody rigidBody;
    private WaitForSeconds dashReInputTime = new WaitForSeconds(0.01f);
    private WaitForSeconds dashDurationTime = new WaitForSeconds(0.35f);
    private WaitForSeconds dashFinishTime = new WaitForSeconds(0.1f);
    private WaitForSeconds dashcoolTime = new WaitForSeconds(0.4f);
    private Coroutine dashCoroutine;
    private bool moveLock = false;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }


    /// <summary>
    /// FixedUpdate()에서 실행할 물리 처리
    /// </summary>
    /// <param name="owner"></param>
    public void UpdateComponent(Controller owner)
    {
        Move(owner);
    }


    private void Move(Controller owner)
    {
        Vector3 newVelocity = owner.theInput.MoveVelocity;
        float moveSpeed = owner.sondol.moveSpeed;

        if (!moveLock)
        {
            if (newVelocity != Vector3.zero)
                LookAt(newVelocity);

            rigidBody.velocity = newVelocity * moveSpeed;
        }
    }

    private void LookAt(Vector3 direction)
    {
        Quaternion targetAngle = Quaternion.LookRotation(direction);
        rigidBody.rotation = targetAngle;
    }


    public void DashMove(Controller owner)
    {
        if (dashCoroutine != null)
        {
            rigidBody.velocity = owner.theInput.MoveVelocity;
            StopCoroutine(dashCoroutine);
        }

        dashCoroutine = StartCoroutine(DashMoveCor(owner));
    }

    private void Dash(Controller owner)
    {
        Vector3 dashVector = owner.theInput.DashVecter;
        Vector3 finalDirection = dashVector != Vector3.zero ? dashVector : rigidBody.transform.forward;
        LookAt(finalDirection);
        owner.theAnimator.DashAnimation(owner);
        rigidBody.velocity = finalDirection * dashPower;
    }

    IEnumerator DashMoveCor(Controller owner)
    {
        moveLock = true;
        owner.canDashAttack = false;

        Dash(owner);
        yield return dashReInputTime;
        owner.isDash = false;
        owner.canDashAttack = true;
        yield return dashDurationTime;
        owner.isDash = true;
        yield return dashFinishTime;
        moveLock = false;
        yield return dashcoolTime;

        owner.isDash = false;
        owner.canDashAttack = false;
        owner.theInput.CurrentDashCount = 0;
    }
}
