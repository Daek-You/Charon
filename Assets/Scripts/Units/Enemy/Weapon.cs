using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class Weapon : MonoBehaviour
{
    public enum Type { Sword, Arrow, Hammer };
    public Type WeaponType;
    public int damage;
   



    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Sondol"))
    //    {
    //        other.gameObject.GetComponent<IHittable>().Damaged(damage);
    //        if(WeaponType == Type.Arrow)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}
}


