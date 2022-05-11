using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Unit
{

 
    void Start()
    {
        InitBaseComponentsSettings();
    }

    void Update()
    {
        LookAtMovingDirection();
    }

    public void Dash()
    {

    }

}
