using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Unit, IHittable
{
    //public int attackDamage;
    protected float rotationSpeed = 15f;
    protected Vector3 destination;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Rigidbody rigidBody;
    protected int moveAnimation = Animator.StringToHash("Move");

    /// <summary>
    /// Enemy 클래스의 NavMeshAgent, Animator, Rigidbody 초기화 함수입니다.
    /// 자식 클래스의 Start() 함수에서 호출해주세요.
    /// </summary>

    protected void InitEnemyComponent()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.isStopped = true;

        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }

    public override void Move(Vector3 destination)
    {
        agent.isStopped = false;
        this.destination = destination;
        animator.SetBool(moveAnimation, true);
        agent.SetDestination(destination);
    }

    /// <summary>
    /// Rigidbody의 isKinematic, NavMeshAgent를 활성화/비활성화합니다.
    /// </summary>
    /// <param name="enable">활성화/비활성화 여부</param>
    public void SetRigidAndNavMeshAgent(bool enable)
    {
        agent.enabled = enable;
        rigidBody.isKinematic = enable;
    }

    public void SetStats(int currentHP, int maxHP, int moveSpeed, int armor/*, int attackDamage*/)
    {
        this.currentHP = currentHP;
        this.maxHP = maxHP;
        this.moveSpeed = moveSpeed;
        this.armor = armor;
        //this.attackDamage = attackDamage;
    }

    protected virtual void LookAtMovingDirection()
    {
        if (!agent.isStopped)
        {
            bool isAlived = agent.velocity.sqrMagnitude >= 0.1f * 0.1f && agent.remainingDistance <= 0.1f;
            bool isMoving = agent.desiredVelocity.sqrMagnitude >= 0.1f * 0.1f;

            if (isAlived)
            {
                agent.isStopped = true;
                animator.SetBool(moveAnimation, false);
            }

            else if (isMoving)
            {
                Vector3 direction = agent.desiredVelocity;
                direction.Set(direction.x, 0f, direction.z);
                Quaternion targetAngle = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
