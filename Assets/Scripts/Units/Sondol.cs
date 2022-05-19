using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sondol : Player
{

    private int dashCount = 0;

    public override void Move(Vector3 vector)
    {

    }


    public void SetStats(int currentHP, int maxHP, int moveSpeed, int armor, int dashCount)
    {
        this.currentHP = currentHP;
        this.maxHP = maxHP;
        this.moveSpeed = moveSpeed;
        this.armor = armor;
        this.dashCount = dashCount;
    }

    public void Dash()
    {

    }

    protected override void OnDie()
    {
    }

    public override void Damaged(int damage)
    {
    }
}
