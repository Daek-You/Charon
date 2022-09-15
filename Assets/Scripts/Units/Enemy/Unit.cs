using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHittable
{
    void Damaged(int damage);
}


public abstract class Unit : MonoBehaviour, IHittable
{

    public int CurrentHP
    {
        get { return currentHP; }
        set
        {
            currentHP = (int)Mathf.Clamp(value, 0, maxHP);

            if (currentHP <= 0)
                OnDie();
        }
    }


    protected int currentHP;
    public int maxHP;
    public int moveSpeed;
    public int armor;

    public abstract void Move(Vector3 vector);
    protected abstract void OnDie();
    public abstract void Damaged(int damage);

}
