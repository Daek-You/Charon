using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sondol : Player
{
    public Rigidbody rigidBody { get; private set; }
    public Animator animator { get; private set; }
    public int dashCount { get; private set; } = 2;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public override void Move(Vector3 direction)
    {
        if (direction != Vector3.zero)
            LookAt(direction);
        rigidBody.position += direction * moveSpeed * Time.deltaTime;
    }

    private void LookAt(Vector3 direction)
    {
        Quaternion targetAngle = Quaternion.LookRotation(direction);
        rigidBody.rotation = targetAngle;
    }

    public void SetStats(int currentHP, int maxHP, int moveSpeed, int armor, int dashCount)
    {
        this.currentHP = currentHP;
        this.maxHP = maxHP;
        this.moveSpeed = moveSpeed;
        this.armor = armor;
        this.dashCount = dashCount;
    }



    public void _Dash(float dashPower, float dashDelay, float stopTime)
    {
        rigidBody.AddForce(rigidBody.transform.forward * dashPower, ForceMode.Impulse);
        StartCoroutine(DashCor(dashPower, dashDelay, stopTime));
    }


    IEnumerator DashCor(float dashPower, float dashDelay, float stopTime)
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashDelay)
        {
            if (Time.time > startTime + stopTime)
                rigidBody.velocity = Vector3.zero;

            yield return null;
        }

        Dash.currentDashCount = 0;
        Move_.enabled = true;
    }


    protected override void OnDie()
    {

    }
    public override void Damaged(int damage)
    {

    }
}
