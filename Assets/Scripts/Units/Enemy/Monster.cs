//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//public enum MonsterType { private_k, private_a, C }

//public class Monster : Enemy
//{
//    public MonsterType MonsterType;
//    //[SerializeField] private Transform target;
//    public Collider HitArea;
//    public bool isChase;
//    public bool isAttack;
//    //bool isDamaged;
//    public Transform arrowPos;
//    public GameObject arrow;


//    // Start is called before the first frame update
//    void Start()
//    {
//        Invoke("ChaseStart", 2);
//    }

//    void ChaseStart()
//    {
//        isChase = true;
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }


//    void Targeting()
//    {
//        float targetRadius = 0;
//        float targetRange = 0;

//        switch (MonsterType)
//        {
//            case MonsterType.private_k:
//                targetRadius = 1.0f;
//                targetRange = agent.stoppingDistance;
//                break;

//            case MonsterType.private_a:
//                targetRadius = 1.5f;
//                targetRange = agent.stoppingDistance + 5;

//                break;
//            case MonsterType.C:
//                targetRadius = 1.5f;
//                targetRange = agent.stoppingDistance;
//                break;
//        }

//        RaycastHit[] rayHits =
//            Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Sondol"));
//        if (rayHits.Length > 0 && !isAttack)
//        {
//            StartCoroutine(Attack());
//        }
//    }

//    IEnumerator Attack()
//    {
//        isChase = false;
//        isAttack = true;
//        animator.SetBool("isAttack", true);
//        switch (MonsterType)
//        {
//            case MonsterType.private_k:
//                yield return new WaitForSeconds(1.0f);
//                break;
//            case MonsterType.private_a:
//                yield return new WaitForSeconds(1.0f);
//                GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
//                Rigidbody rigidArrow = instantArrow.GetComponent<Rigidbody>();
//                Destroy(instantArrow, 5.0f);
//                yield return new WaitForSeconds(0.7f);
//                rigidArrow.velocity = transform.forward * 10;
//                yield return new WaitForSeconds(1.0f);
//                break;
//            case MonsterType.C:
//                yield return new WaitForSeconds(3.0f);
//                break;
//        }
//        isAttack = false;
//        animator.SetBool("isAttack", false);
//        isChase = true;
//    }

//    void FixedUpdate()
//    {
//        Targeting();
//    }


//    private void OnTriggerEnter(Collider other)
//    {
//        if (currentHP > 0 && !isDamaged)
//        {
//            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
//            {
//                //Damaged(10);
//                //StartCoroutine(Hitted());
//            }
//        }
//    }
//    //IEnumerator Hitted()
//    //{
//    //    isDamaged = true;
//    //    animator.SetTrigger("Damaged");
//    //    Damaged(10);
//    //    yield return new WaitForSeconds(0.2f);
//    //    isDamaged = false;
//    //    ;
//    //}

//    //currentHp<0ÀÏ°æ¿ì
//    //protected override void OnDie()
//    //{
//    //    gameObject.layer = 9;
//    //    isDamaged = false;
//    //    isChase = false;
//    //    isAttack = false;
//    //    //agent.enabled = false;
//    //    //animator.SetBool("isAttack", false);
//    //    //animator.SetBool(moveAnimation, false);
//    //    animator.SetTrigger("doDie");
//    //    Destroy(gameObject, 3);
//    //}

//    //public override void Damaged(int damage)
//    //{
//    //    CurrentHP -= damage;    
//    //}

//}
