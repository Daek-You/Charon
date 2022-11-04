using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    //�̱������� ����.
    private static StatManager instance = null;
    public static StatManager Instance { get { return instance; } }

    private Stat maxHP;            //�ִ�ü��
    private Stat moveSpeed;        //�̵��ӵ�
    private Stat attackDamage;     //���ܷ�. ������ ������ ���� �޶���.
    private Stat armor;            //����
    private Stat dashCount;        //�뽬 Ƚ��


    //�ٸ� ��ũ��Ʈ���� ������ ���� ����
    public float currentMaxHP { get { return maxHP.currentValue; } }
    public float currentMoveSpeed { get { return moveSpeed.currentValue; } }
    public float currentAttackDamage { get { return attackDamage.currentValue; } }
    public float currentArmor { get { return armor.currentValue; } }
    public int currentDashCount { get { return (int)dashCount.currentValue; } }

    //�̱��� ���� ����.
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
        //���Ⱑ �ٲ���� �� ��� �ִ� ���⿡ ���� attack damage�� �ҷ����� �Լ� �ʿ�.
        //Player.Instance.weaponManager.Weapon.Name �̷������� �޾ƿ��� ��.
    }

    //�ʱ� ���� ����.
    public void InitStatus()
    {
        maxHP = new Stat(StatType.MAX_HP, initValue: 1000, increasingAmount: 200, currentReinforceLevel: 0, maxReinforceLevel: 10);
        moveSpeed = new Stat(StatType.MOVE_SPEED, initValue: 3, increasingAmount: 0.2f, currentReinforceLevel: 0, maxReinforceLevel: 10);
        dashCount = new Stat(StatType.DASH_COUNT, initValue: 1, increasingAmount: 1, currentReinforceLevel: 0, maxReinforceLevel: 2);
        //attackDamage = new Stat(StatType.ATTACK_DAMAGE, initValue: 100, increasingAmount: 20, currentReinforceLevel: 0, maxReinforceLevel: 10);
        armor = new Stat(StatType.ARMOR, initValue: 50, increasingAmount: 10f, currentReinforceLevel: 0, maxReinforceLevel: 10);
    }

    //��ȭ�� ������ ���� ���ȿ� �����Ű�� �Լ�.
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
        //��Ŀ�� ���� ��� �̷��� ���Կ�
        

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
