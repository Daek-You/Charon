using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected string interactionText = "";
    public string InteractionText { get { return interactionText; } }

    public abstract void interact();
}
