using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State
{
    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();

}


public class IdleState : State
{

    public override void Enter()
    {

    }

    public override void Update() { }
    public override void FixedUpdate() { }
    public override void Exit()
    {

    }
}
