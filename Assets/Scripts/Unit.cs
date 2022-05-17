using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class Unit : MonoBehaviour
{
    protected float rotationSpeed = 15;
    protected Vector3 destination;
    protected NavMeshAgent agent;
    protected Rigidbody rigidBody;
    protected Animator animator;
    private int moveAnimation = Animator.StringToHash("Move");

    public virtual void Move(Vector3 destination)
    {
        agent.isStopped = false;
        this.destination = destination;
        agent.SetDestination(destination);
        animator.SetBool(moveAnimation, true);
    }

    public virtual void LookAtMovingDirection()   // 보간적으로 회전하는 것이기 때문에 Update() 함수에서 돌릴 것
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
                direction.Set(direction.x, 0f, direction.z);        // 경사 지형 위에 올라갔을 때, 캐릭터가 경사 지형에 따라 회전하는 걸 방지
                Quaternion targetAngle = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, rotationSpeed * Time.deltaTime);
            }
        }
    }


    protected void InitBaseComponentsSettings()  // 자식 클래스 Start() 함수에서 실행하여 초기화 시켜줄 것
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        agent.updateRotation = false;
        agent.isStopped = true;
    }
}
