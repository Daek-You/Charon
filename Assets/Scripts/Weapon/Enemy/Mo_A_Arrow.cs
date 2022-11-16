using CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mo_A_Arrow : MonoBehaviour
{
    public Enemy owner;
    public Rigidbody rigidBody;
    public const float MAX_DISTANCE = 15f;
    private Coroutine meterCalculatedCoroutine;
    private Char_Private_A_Weapon weapon;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sondol") && weapon != null)
        {
            IHittable target = other.gameObject.GetComponent<IHittable>();
            target?.Damaged(owner.Weapon.AttackDamage);
        }
    }

    public void StartCalculatingMeter()
    {
        if (meterCalculatedCoroutine != null)
            StopCoroutine(meterCalculatedCoroutine);

        meterCalculatedCoroutine = StartCoroutine(CalculateMeter());
    }

    private IEnumerator CalculateMeter()
    {
        while (true)
        {
            if(Vector3.Distance(transform.position, owner.transform.position) > MAX_DISTANCE)
            {
                if (weapon == null)
                    weapon = owner.Weapon as Char_Private_A_Weapon;
                weapon.Retrieve(this);
                break;
            }
            yield return null;
        }
    }
}
