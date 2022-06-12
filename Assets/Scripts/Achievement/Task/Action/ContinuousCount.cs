using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Task/TaskAction/ContinuousCount", fileName = "ContinuousCount")]
public class ContinuousCount : TaskAction
{
    public override int Run(Task task, int currentSuccess, int successCount)
    {
        if (successCount > 0)
            return currentSuccess + successCount;
        else
            return 0;
    }
}
