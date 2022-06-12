using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class _Physics : MonoBehaviour, IComponent<Controller>
{

    public float dashPower;
    public bool moveLock { get; set; } = false;
    public Coroutine dashCoroutine { get; private set; }

    private Rigidbody rigidBody;
    private WaitForSeconds dashReInputTime = new WaitForSeconds(0.01f);
    private WaitForSeconds dashDurationTime = new WaitForSeconds(0.35f);
    private WaitForSeconds dashFinishTime = new WaitForSeconds(0.2f);
    private WaitForSeconds dashcoolTime = new WaitForSeconds(0.3f);
    private WaitForSeconds dashAttackDuration = new WaitForSeconds(0.15F);
    private WaitForSeconds dashAttackCoolTime = new WaitForSeconds(0.5f);

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

    public void LookAt(Vector3 direction)
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
        owner.theAnimator.DashAnimation();
        rigidBody.velocity = finalDirection * dashPower;
    }

    IEnumerator DashMoveCor(Controller owner)
    {
        moveLock = true;
        Dash(owner);

        yield return dashReInputTime;
        owner.canInputKey = true;

        yield return dashDurationTime;
        owner.canInputKey = false;

        yield return dashFinishTime;
        moveLock = false;
        Debug.Log("대시 락 풀림");
        yield return dashcoolTime;
        owner.isDash = false;
        owner.theInput.CurrentDashCount = 0;
    }



    public IEnumerator DashAttackCor(Controller owner, Vector3 mouseDirection)
    {
        InitialDashSetting(owner);
        
        moveLock = true;
        LookAt(mouseDirection);
        owner.theAnimator.DashAttackAnimation(true);
        rigidBody.velocity = mouseDirection * dashPower * 2f;
        owner.theInput.moveInputLock = true;    // 키 입력 받지 못 하도록

        yield return dashAttackDuration;
        rigidBody.velocity = Vector3.zero;
        owner.theAnimator.DashAttackAnimation(false);

        yield return dashAttackCoolTime;
        moveLock = false;
        owner.theInput.moveInputLock = false;
        owner.isDashAttack = false;
    }


    private void InitialDashSetting(Controller owner)
    {
        moveLock = false;
        owner.canInputKey = false;
        owner.isDash = false;
        owner.theInput.CurrentDashCount = 0;
    }
}
