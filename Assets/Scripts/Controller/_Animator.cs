using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class _Animator : MonoBehaviour, IComponent<Controller>
{

    public int moveAnimation { get; private set; }
    public int dashAnimation { get; private set; }

    private Animator animator;
    private float moveAnimationPlaySpeed = 0.9f;


    void Awake()
    {
        animator = GetComponent<Animator>();
        moveAnimation = Animator.StringToHash("Move");
        dashAnimation = Animator.StringToHash("Dash");
    }

    public void UpdateComponent(Controller owner)
    {
        MoveAnimation(owner);
    }

    public void MoveAnimation(Controller owner)
    {
        float isMove = (owner.theInput.MoveVelocity != Vector3.zero) ? moveAnimationPlaySpeed : 0f;
        animator.SetFloat(moveAnimation, isMove);
    }

    public void DashAnimation(Controller owner)
    {
        animator.SetTrigger(dashAnimation);
    }
}
