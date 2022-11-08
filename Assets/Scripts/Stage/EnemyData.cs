using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    Enemy type;
    Vector3 position;

    public EnemyData(Enemy type, Vector3 position)
    {
        this.type = type;
        this.position = position;
    }
}
