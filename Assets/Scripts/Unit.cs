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

    public virtual void LookAtMovingDirection()   // ���������� ȸ���ϴ� ���̱� ������ Update() �Լ����� ���� ��
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
                direction.Set(direction.x, 0f, direction.z);        // ��� ���� ���� �ö��� ��, ĳ���Ͱ� ��� ������ ���� ȸ���ϴ� �� ����
                Quaternion targetAngle = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, rotationSpeed * Time.deltaTime);
            }
        }
    }


    protected void InitBaseComponentsSettings()  // �ڽ� Ŭ���� Start() �Լ����� �����Ͽ� �ʱ�ȭ ������ ��
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        agent.updateRotation = false;
        agent.isStopped = true;
    }
}
