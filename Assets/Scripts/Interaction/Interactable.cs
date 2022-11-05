using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected string targetText = "��ȣ�ۿ�";
    public string TargetText { get { return targetText; } }

    public abstract void interact();
}
