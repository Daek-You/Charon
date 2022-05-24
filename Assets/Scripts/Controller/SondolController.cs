using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SondolController : MonoBehaviour
{
    public static bool EnabledInput = true;
    [SerializeField] Sondol owner;
    private ICommand<Sondol> moveCommand;
    private ICommand<Sondol> dashCommand;


    void Awake()
    {
        moveCommand = new Move_();
        dashCommand = new Dash();
    }

    void Update()
    {
        if (EnabledInput)
        {
            moveCommand.InputUpdate(owner);
            dashCommand.InputUpdate(owner);
        }
    }

    void FixedUpdate()
    {
        moveCommand.PhysicsUpdate(owner);
        dashCommand.PhysicsUpdate(owner);
    }
}

