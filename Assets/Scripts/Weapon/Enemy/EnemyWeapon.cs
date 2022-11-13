using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour
{
    public float AttackDamage { get { return attackDamage; } }
    [SerializeField] protected Enemy owner;
    [SerializeField] protected float attackDamage;



    public abstract void Attack();
    public abstract void StopAttack();

    protected void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Sondol"))
        {
            HitState hitState = Player.Instance.stateMachine.GetState(CharacterController.StateName.HIT) as HitState;

            if (Player.Instance.stateMachine.CurrentState == hitState)
                return;

            Player.Instance.Damaged(attackDamage);
        }
    }
}
