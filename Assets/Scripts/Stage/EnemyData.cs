using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    Enemy type;
    int count;
    Vector3[] position;

    public EnemyData(Enemy type, int count, Vector3[] position)
    {
        this.type = type;
        this.count = count;
        this.position = position;
    }
}
