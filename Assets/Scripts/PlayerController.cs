using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{

    [SerializeField] private Player player;
    private Command mouseRClick;

    void Awake()
    {
        mouseRClick = new CommandMouseRClick();
    }

    private Command GetCommand()
    {
        if (Input.GetMouseButton(1))
            return mouseRClick;
        return null;
    }

    void Update()
    {
        Command command = GetCommand();
        command?.Execute(player);
    }
}

