using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CharonPaddle : MonoBehaviour, IWeapon
{

    public int attackDamage;
    public float attackSpeed;
    public const int MAX_COMBO_COUNT = 3;
    private Coroutine dashAttackCor;
    private Coroutine attackCor;
    private WaitForSeconds attackCoolTime = new WaitForSeconds(0.1f);

    public void Attack(Controller owner, Vector3 mouseDirection)
    {
        if (!owner.isAttack || owner.canComboAttack)
        {
            owner.isAttack = true;
            owner.canComboAttack = false;
            owner.theInput.moveInputLock = true;
            owner.thePhysics.moveLock = true;

            owner.thePhysics.LookAt(mouseDirection);
            owner.theAnimator.SetAttackAnimationSpeed(attackSpeed);
            owner.theAnimator.AttackAnimation();


            //if (attackCor != null)
            //    StopCoroutine(attackCor);
            //attackCor = StartCoroutine(AttackCor(owner, mouseDirection));
        }
    }

    public void DashAttack(Controller owner, Vector3 mouseDirection)
    {
        owner.isDashAttack = true;
        if (mouseDirection != Vector3.zero)
        {
            IEnumerator dashAttckCoroutine = owner.thePhysics.DashAttackCor(owner, mouseDirection);

            if (dashAttckCoroutine != null)
                StopCoroutine(dashAttckCoroutine);

            dashAttackCor = StartCoroutine(dashAttckCoroutine);
        }
    }

    public void ChargeAttack(Controller owner, Vector3 mouseDirection)
    {

    }

    public void Skill(Controller owner, Vector3 mouseDirection)
    {

    }

    public void UltimateSkill(Controller owner, Vector3 mouseDirection)
    {

    }


    //private IEnumerator AttackCor(Controller owner, Vector3 mouseDirection)
    //{
    //    owner.isAttack = true;
    //    owner.canComboAttack = false;
    //    owner.theInput.moveInputLock = true;
    //    owner.thePhysics.moveLock = true;

    //    owner.thePhysics.LookAt(mouseDirection);
    //    owner.theAnimator.SetAttackAnimationSpeed(attackSpeed);
    //    owner.theAnimator.AttackAnimation();

    //    while (owner.theAnimator.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.25f)
    //        yield return null;

    //    owner.canComboAttack = true;
    //    Debug.Log("ÄÞº¸ °¡´É");

    //    while (owner.theAnimator.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
    //        yield return null;

    //    owner.canComboAttack = false;


    //    yield return attackCoolTime;
    //    //Debug.Log("Lock off");
    //    //owner.theInput.moveInputLock = false;
    //    // owner.thePhysics.moveLock = false;
    //    //owner.isAttack = false;
    //}
}