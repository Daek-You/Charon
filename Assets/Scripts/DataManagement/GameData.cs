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
    private float currentHP;
    [SerializeField]
    private int currentST;
    [SerializeField]
    private string weaponName;
    [SerializeField]
    private int currentWeaponReinforceLevel;
    [SerializeField]
    private int gold = 0;
    [SerializeField]
    private int currentHPReinforceLevel = 0;
    [SerializeField]
    private int currentArmorReinforceLevel = 0;
    [SerializeField]
    private int currentMoveSpeedReinforceLevel = 0;
    [SerializeField]
    private int currentDashCountReinforceLevel = 0;
    [SerializeField]
    private List<string> reinforceWeaponList;
    [SerializeField]
    private List<int> reinforceWeaponValueList;

    public Vector3 CurrentPosition { get { return currentPosition; } set { currentPosition = value; } }
    public float CurrentHP { get { return currentHP; } set { currentHP = value; } }
    public int CurrentST { get { return currentST; } set { currentST = value; } }
    public string WeaponName { get { return weaponName; } set { weaponName = value;} }
    public int CurrentWeaponReinforecLevel { get { return currentWeaponReinforceLevel; } set { currentWeaponReinforceLevel = value; } }
    public int Gold { get { return gold; } set { gold = value; } }
    public int CurrentHPReinforceLevel { get { return currentHPReinforceLevel; } set { currentHPReinforceLevel = value; } }
    public int CurrentArmorReinforceLevel { get { return currentArmorReinforceLevel; } set { currentArmorReinforceLevel = value; } }
    public int CurrentMoveSpeedReinforceLevel { get { return currentMoveSpeedReinforceLevel; } set { currentMoveSpeedReinforceLevel = value; } }
    public int CurrentDashCountReinforceLevel { get { return currentDashCountReinforceLevel; } set { currentDashCountReinforceLevel = value; } }
    public List<string> ReinforceWeaponList { get { return reinforceWeaponList; } set { reinforceWeaponList = value; } }
    public List<int> ReinforceWeaponValueList { get { return reinforceWeaponValueList; } set { reinforceWeaponValueList = value; } }
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
