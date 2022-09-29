using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Handle Data", menuName = "Scriptable Object/Weapon Handle Data", order = int.MaxValue)]
public class WeaponHandleData : ScriptableObject
{
    public Vector3 localPosition;
    public Vector3 localRotation;
    public Vector3 localScale;
}