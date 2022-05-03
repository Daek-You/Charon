using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SondolController : MonoBehaviour
{

    [SerializeField] Transform character;
    [SerializeField] float rotationSpeed;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 destination;
    private bool isMove = false;
    private int walkableLayer;
    private int moveAnimation;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        moveAnimation = Animator.StringToHash("Move");
        agent.updateRotation = false;                            // Navigation ȸ��ó�� ��Ȱ��ȭ
        walkableLayer = 1 << LayerMask.NameToLayer("Walkable");  // Walkable ���̾ Ž��
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity, walkableLayer))
            {
                SetDestination(hitInfo.point);
            }
        }
        LookAt();
    }


    private void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
        this.destination = destination;
        isMove = true;
        animator.SetBool(moveAnimation, true);
    }


    private void LookAt()
    {
        if (isMove)
        {
            bool isAlived = agent.velocity.sqrMagnitude >= 0.1f * 0.1f && agent.remainingDistance <= 0.1f;
            bool isMoving = agent.desiredVelocity.sqrMagnitude >= 0.1f * 0.1f;

            if (isAlived)
            {
                isMove = false;
                animator.SetBool(moveAnimation, false);
            }
            else if (isMoving)
            {
                Vector3 direction = agent.desiredVelocity;
                direction.Set(direction.x, 0f, direction.z);   // ����� �� ���� �ö��� �� ȸ�� ����
                Quaternion targetAngle = Quaternion.LookRotation(direction);
                character.rotation = Quaternion.Slerp(character.rotation, targetAngle, rotationSpeed * Time.deltaTime);
            }
        }
    }

}

