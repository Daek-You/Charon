using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Private_K : Enemy
{

    void OnEnable()
    {
        currentHP = maxHP;
        stateMachine?.ChangeState(CharacterController.StateName.ENEMY_MOVE);
    }

    void Start()
    {
        InitSettings();

    }
}
