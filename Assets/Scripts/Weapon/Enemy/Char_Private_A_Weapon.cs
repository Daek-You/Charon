using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Private_A_Weapon : EnemyWeapon
{
    public readonly int attackAnimation = Animator.StringToHash("IsAttack");
    public Queue<GameObject> arrowPool { get; private set; } = new Queue<GameObject>();

    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform arrowCreatePosition;
    [SerializeField] private float arrowMoveSpeed;
    [SerializeField] private int maxArrowCount;

    void Awake()
    {
        for(int i = 0; i < maxArrowCount; i++)
        {
            GameObject obj = Instantiate(arrow) as GameObject;
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            arrowPool.Enqueue(obj);
        }
    }

    public override void Attack()
    {
        owner.animator.SetBool(attackAnimation, true);
    }

    public override void StopAttack()
    {
        owner.animator.SetBool(attackAnimation, false);
    }

    public void Shot()
    {
        if(arrowPool.Count > 0)
        {
            GameObject myArrow = arrowPool.Dequeue();
            myArrow.transform.SetParent(null);

            Vector3 direction = (Player.Instance.transform.position - owner.transform.position).normalized;
            myArrow.transform.position = arrowCreatePosition.position;
            myArrow.transform.rotation = Quaternion.LookRotation(direction);
            myArrow.SetActive(true);

            Mo_A_Arrow arrowScript = myArrow.GetComponent<Mo_A_Arrow>();
            arrowScript.rigidBody.velocity = direction * arrowMoveSpeed;
            arrowScript.StartCalculatingMeter();
        }
    }

    public void Retrieve(Mo_A_Arrow arrow)
    {
        arrow.transform.SetParent(transform);
        arrowPool.Enqueue(arrow.gameObject);
        arrow.rigidBody.velocity = Vector3.zero;
        arrow.gameObject.SetActive(false);
    }
}

