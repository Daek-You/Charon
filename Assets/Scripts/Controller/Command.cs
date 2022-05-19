using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public virtual void Execute(Player player) { }
}


public class CommandMouseRClick : Command
{
    private int checkingLayer = 1 << LayerMask.NameToLayer("Walkable");
    // ���Ŀ� ����, NPC ���̾ �߰��ؾ� �� �� ����

    public override void Execute(Player player)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, checkingLayer))
        {
            player.Move(hitInfo.point);
        }
    }
}

