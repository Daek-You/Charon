using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    //싱글톤으로 구현.
    private static StatManager instance = null;
    public static StatManager Instance { get { return instance; } }

    private Stat maxHP;            //최대체력
    private Stat moveSpeed;        //이동속도
    private Stat attackDamage;     //공겨력. 무기의 종류에 따라 달라짐.
    private Stat armor;            //방어력
    private Stat dashCount;        //대쉬 횟수


    //다른 스크립트에서 참조할 현재 스탯
    public float currentMaxHP { get { return maxHP.currentValue; } }
    public float currentMoveSpeed { get { return moveSpeed.currentValue; } }
    public float currentAttackDamage { get { return attackDamage.currentValue; } }
    public float currentArmor { get { return armor.currentValue; } }
    public int currentDashCount { get { return (int)dashCount.currentValue; } }

    //싱글톤 관련 구문.
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        DestroyImmediate(this.gameObject);
    }

    void Start()
    {
        InitStatus();
    }
    private void Update()
    {
        //무기가 바뀌었을 때 들고 있는 무기에 따라 attack damage를 불러오는 함수 필요.
        //Player.Instance.weaponManager.Weapon.Name 이런식으로 받아오면 됨.
    }

    //초기 스탯 생성.
    public void InitStatus()
    {
        maxHP = new Stat(StatType.MAX_HP, initValue: 1000, increasingAmount: 200, currentReinforceLevel: 0, maxReinforceLevel: 10);
        moveSpeed = new Stat(StatType.MOVE_SPEED, initValue: 3, increasingAmount: 0.2f, currentReinforceLevel: 0, maxReinforceLevel: 10);
        dashCount = new Stat(StatType.DASH_COUNT, initValue: 1, increasingAmount: 1, currentReinforceLevel: 0, maxReinforceLevel: 2);
        //attackDamage = new Stat(StatType.ATTACK_DAMAGE, initValue: 100, increasingAmount: 20, currentReinforceLevel: 0, maxReinforceLevel: 10);
        armor = new Stat(StatType.ARMOR, initValue: 50, increasingAmount: 10f, currentReinforceLevel: 0, maxReinforceLevel: 10);
    }

    //강화된 스탯을 현재 스탯에 적용시키는 함수.
    public void SetStatus(StatType statType)
    {
        switch (statType)
        {
            case StatType.MAX_HP:
                maxHP.currentValue = maxHP.initValue + maxHP.increasingAmount * maxHP.currentReinforceLevel;
                Debug.Log(currentMaxHP);
                return;
            case StatType.MOVE_SPEED:
                moveSpeed.currentValue = moveSpeed.initValue + moveSpeed.increasingAmount * moveSpeed.currentReinforceLevel;
                Debug.Log(currentMoveSpeed);
                return;
            case StatType.DASH_COUNT:
                dashCount.currentValue = dashCount.initValue + dashCount.increasingAmount * dashCount.currentReinforceLevel;
                Debug.Log(currentDashCount);
                return;
            case StatType.ARMOR:
                armor.currentValue = armor.initValue + armor.increasingAmount * armor.currentReinforceLevel;
                Debug.Log(currentArmor);
                return;
            default:
                Player.Instance.OnUpdateStat(currentMaxHP, currentMaxHP, currentArmor, currentMoveSpeed, currentDashCount); 
                return;
        }
    }

    public void UpgradeStatus(StatType statType)
    {
        //해커톤 때만 잠깐만 이렇게 쓸게요
        

        switch (statType)
        {
            case StatType.MAX_HP:
                if (maxHP.currentReinforceLevel < maxHP.maxReinforceLevel)
                    maxHP.currentReinforceLevel++;
                SetStatus(statType);
                return;
            case StatType.ARMOR:
                if (armor.currentReinforceLevel < armor.maxReinforceLevel)
                    armor.currentReinforceLevel++;
                SetStatus(statType);
                return;
            case StatType.MOVE_SPEED:
                if (moveSpeed.currentReinforceLevel < moveSpeed.maxReinforceLevel)
                    moveSpeed.currentReinforceLevel++;
                SetStatus(statType);
                return;
            case StatType.DASH_COUNT:
                if (dashCount.currentReinforceLevel < dashCount.maxReinforceLevel)
                    dashCount.currentReinforceLevel++;
                SetStatus(statType);
                return;
            case StatType.PADDLE_DAMAGE:
                WeaponUpgradeFunction();
                return;
            //case StatType.SICKLE_DAMAGE:
            //    if (dashCount.currentReinforceLevel < dashCount.maxReinforceLevel)
            //        dashCount.currentReinforceLevel++;
            //    SetStatus(statType);
            //    return;
            //case StatType.GOURD_DAMAGE:
            //    if (dashCount.currentReinforceLevel < dashCount.maxReinforceLevel)
            //        dashCount.currentReinforceLevel++;
            //    SetStatus(statType);
        }
    }

    void WeaponUpgradeFunction()
    {
        float CurrentWeaponDamage = Player.Instance.weaponManager.Weapon.AttackDamage;
        string CurrentWeaponName = Player.Instance.weaponManager.Weapon.Name;
        float CurrentWeaponAttackSpeed = Player.Instance.weaponManager.Weapon.AttackSpeed;
        float CurrentWeaponAttackRange = Player.Instance.weaponManager.Weapon.AttackRange;

        Player.Instance.weaponManager.Weapon.SetWeaponData(CurrentWeaponName, CurrentWeaponDamage, CurrentWeaponAttackSpeed, CurrentWeaponAttackRange);
    }
}
