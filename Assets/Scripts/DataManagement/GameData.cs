using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField]
    private bool isSaved = false;
    public bool IsSaved { get { return isSaved; } set { isSaved = value; } }

    #region User
    [SerializeField]
    private Vector3 currentPosition = Vector3.zero;
    [SerializeField]
    private string weaponName;
    [SerializeField]
    private int gold = 0;

    public Vector3 CurrentPosition { get { return currentPosition; } set { currentPosition = value; } }
    public string WeaponName { get { return weaponName; } set { weaponName = value;} }
    public int Gold { get { return gold; } set { gold = value; } }
    #endregion

    #region Stage
    [SerializeField]
    private StageType currentStage = StageType.Unknown;
    [SerializeField]
    private bool isCleared = false;

    public StageType CurrentStage { get { return currentStage; } set { currentStage = value; } }
    public bool IsCleared { get { return isCleared; } set { isCleared = value; } }
    #endregion
}
