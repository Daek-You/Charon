using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get { return instance; } }
    private static Player instance;

    #region #캐릭터 스탯 프로퍼티
    public float MaxHP     { get { return maxHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float Armor     { get { return armor; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float DashCount { get { return dashCount; } }
    #endregion

    #region #캐릭터 스탯
    [Header("캐릭터 스탯")]
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float armor;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float dashCount;
    #endregion

    #region #Unity 함수
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }
    #endregion

    public void OnUpdateStat(float maxHP, float currentHP, float armor, float moveSpeed, float dashCount)
    {
        this.maxHP = maxHP;
        this.currentHP = currentHP;
        this.armor = armor;
        this.moveSpeed = moveSpeed;
        this.dashCount = dashCount;
    }
}
