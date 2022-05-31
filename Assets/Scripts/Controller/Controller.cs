using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Controller : MonoBehaviour
{
    public static bool EnabledInput = true;

    public Sondol sondol { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public Animator animator { get; private set; }

    private WaitForSeconds tetanyTime = new WaitForSeconds(0.05f);
    private ICommand<Controller> moveCommand;
    private ICommand<Controller> dashCommand;
    private Coroutine timer;
    private Coroutine dash;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        sondol = GetComponent<Sondol>();
        moveCommand = new Move_();
        dashCommand = new Dash();
    }

    void Update()
    {
        if (EnabledInput)
        {
            moveCommand.InputUpdate(this);
            dashCommand.InputUpdate(this);
        }
    }

    void FixedUpdate()
    {
        moveCommand.PhysicsUpdate(this);
        dashCommand.PhysicsUpdate(this);
    }

    public void Move()
    {
        if (Move_.velocity != Vector3.zero)
            LookAt(Move_.velocity);
        rigidBody.velocity = Move_.velocity * sondol.moveSpeed;
    }

    private void LookAt(Vector3 direction)
    {
        Quaternion targetAngle = Quaternion.LookRotation(direction);
        rigidBody.rotation = targetAngle;
    }


    public void DashMove(int dashAnimation, float dashPower, float conditionalTime, float dashDuration)
    {
        if (dash != null)
        {
            rigidBody.velocity = Move_.velocity;
            StopCoroutine(dash);
        }
        dash = StartCoroutine(DashCor(dashAnimation, dashPower, conditionalTime, dashDuration));
    }

    private IEnumerator DashCor(int dashAnimation, float dashPower, float conditionalTime, float dashDuration)
    {
        _Dash(dashAnimation, dashPower);
        yield return new WaitForSeconds(0.1f);
        Dash.IsDash = false;
        yield return new WaitForSeconds(0.3f);
        Dash.IsDash = true;
        yield return new WaitForSeconds(0.3f);

        Dash.Lock = false;
        Dash.currentDashCount = 0;
        Dash.IsDash = false;

    }


    private void _Dash(int dashAnimation, float dashPower)
    {
        Vector3 finalDirection = Dash.dashVector != Vector3.zero ? Dash.dashVector : rigidBody.transform.forward;
        LookAt(finalDirection);
        animator.SetTrigger(dashAnimation);
        rigidBody.velocity = finalDirection * dashPower;
    }

    //public void TimeCheck(float startTime, float conditionalTime)
    //{
    //    if (timer != null)
    //        StopCoroutine(timer);
    //    timer = StartCoroutine(TimeCheckCor(startTime, conditionalTime));
    //}

    //private IEnumerator TimeCheckCor(float startTime, float conditionalTime)
    //{
    //    Dash.IsTimeOver = false;

    //    while (Time.time < startTime + conditionalTime)
    //    {
    //        yield return null;
    //    }

    //    Dash.IsTimeOver = true;
    //    Debug.Log("타임오버");
    //}
}

