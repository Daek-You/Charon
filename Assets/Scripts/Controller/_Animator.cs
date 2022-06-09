using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class _Animator : MonoBehaviour, IComponent<Controller>
{

    public int _MoveAnimation { get; private set; }
    public int _DashAnimation { get; private set; }
    public int _DashAttackAnimation { get; private set; }

    private Animator animator;
    private float moveAnimationPlaySpeed = 0.9f;


    void Awake()
    {
        animator = GetComponent<Animator>();
        _MoveAnimation = Animator.StringToHash("Move");
        _DashAnimation = Animator.StringToHash("Dash");
        _DashAttackAnimation = Animator.StringToHash("IsDashAttack");
    }

    public void UpdateComponent(Controller owner)
    {
        MoveAnimation(owner);
    }

    public void MoveAnimation(Controller owner)
    {
        float isMove = (owner.theInput.MoveVelocity != Vector3.zero) ? moveAnimationPlaySpeed : 0f;
        animator.SetFloat(_MoveAnimation, isMove);
    }

    public void DashAnimation()
    {
        animator.ResetTrigger(_DashAnimation);
        animator.SetTrigger(_DashAnimation);
    }

    public void DashAttackAnimation(bool enable)
    {
        animator.SetBool(_DashAttackAnimation, enable);
    }
}
