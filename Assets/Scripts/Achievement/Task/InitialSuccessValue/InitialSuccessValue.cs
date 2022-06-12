using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InitialSuccessValue : ScriptableObject
{
    public abstract int GetValue(Task task);
}
