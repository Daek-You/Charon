using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class Player : MonoBehaviour
{
    public static Player Instance { get { return instance; } }
    public WeaponManager weaponManager { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public PlayerController Controller { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }

    public AnimationEventHandler _AnimationEventHandler { get; private set; }


    public Transform effectGenerator;

    [SerializeField]
    private Transform rightHand;
    private static Player instance;


    #region #캐릭터 스탯
    public float MaxHP     { get { return maxHP; } }
    public float CurrentHP { get { return currentHP; } }
    public float Armor     { get { return armor; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public int DashCount { get { return dashCount; } }

    [Header("캐릭터 스탯")]
    [SerializeField] protected float maxHP;
    [SerializeField] protected float currentHP;
    [SerializeField] protected float armor;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int dashCount;
    #endregion

    #region #Unity 함수
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            weaponManager = new WeaponManager(rightHand);
            weaponManager.unRegisterWeapon = (weapon) => { Destroy(weapon); };
            rigidBody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            Controller = GetComponent<PlayerController>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            _AnimationEventHandler = GetComponent<AnimationEventHandler>();
            InitStateMachine();
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }


    void Start()
    {
        //animator.SetFloat("AttackSpeed", weaponManager.Weapon.AttackSpeed);
        
    }

    void Update()
    {
        stateMachine?.UpdateState();
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }
    #endregion

    public void OnUpdateStat(float maxHP, float currentHP, float armor, float moveSpeed, int dashCount)
    {
        this.maxHP = maxHP;
        this.currentHP = currentHP;
        this.armor = armor;
        this.moveSpeed = moveSpeed;
        this.dashCount = dashCount;
    }

    private void InitStateMachine()
    {
        stateMachine = new StateMachine(StateName.MOVE, new MoveState());
        stateMachine.AddState(StateName.DASH, new DashState(dashPower: 2f, dashTetanyTime: 0.5f, dashCoolTime: 0.5f));
        stateMachine.AddState(StateName.ATTACK, new AttackState());
        stateMachine.AddState(StateName.DASH_ATTACK, new DashAttackState());
        stateMachine.AddState(StateName.CHARGING, new ChargingState());
        stateMachine.AddState(StateName.CHARGING_ATTACK, new ChargingAttackState());
        stateMachine.AddState(StateName.SKILL, new SkillState());
    }
}
