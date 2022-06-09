using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sondol : Unit
{

    public int dashCount;
    public IWeapon myWeapon { get; private set; }




    void Start()
    {
        myWeapon = GetComponentInChildren<IWeapon>();
        Debug.Log(myWeapon);
    }



    /// <summary>
    /// 사용하지 않습니다.
    /// </summary>
    /// <param name="vector"></param>
    public override void Move(Vector3 vector) { }

    public void SetStats(int currentHP, int maxHP, int moveSpeed, int armor, int dashCount)
    {
        this.currentHP = currentHP;
        this.maxHP = maxHP;
        this.moveSpeed = moveSpeed;
        this.armor = armor;
        this.dashCount = dashCount;
    }

    protected override void OnDie()
    {

    }
    public override void Damaged(int damage)
    {

    }
}