using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private static InventoryManager instance;
    public GameObject charonPaddle;  /// 테스트용

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

    void Start()
    {
        Init();
    }


    private void Init()
    {
        /* 
           ---------슈도 코드---------
           GameObject[] weapons = Database.LoadWeapons();
           for(int i = 0; i < weapons.Length; i++)
           {
                Player.Instance.weaponManager.RegisterWeapon(weapon[i]);
           }
        */
        GameObject weapon = Instantiate(charonPaddle);
        Player.Instance.weaponManager.RegisterWeapon(weapon);
        Player.Instance.weaponManager.SetWeapon(weapon);
    }
}
