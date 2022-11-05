using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected string targetText = "상호작용";
    public string TargetText { get { return targetText; } }

    public abstract void interact();
}
