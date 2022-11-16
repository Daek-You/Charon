using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class Player : MonoBehaviour, IHittable
{
    public bool IsDied { get; private set; } = false;
    public static Player Instance { get { return instance; } }
    public WeaponManager weaponManager { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public PlayerController Controller { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }
    public SkinnedMeshRenderer skinnedMeshRenderer { get; private set; }
    public Color originalMaterialColor { get; private set; }
    public AnimationEventHandler _AnimationEventHandler { get; private set; }
    public AudioSource audioSource { get; private set; }

    public Transform effectGenerator;

    public GameObject hitBox;


    [SerializeField]
    private Transform rightHand;
    private static Player instance;
    [SerializeField] AudioClip hitSound;

    float ACTIVE_TIME = 3.0f;

    #region #캐릭터 스탯
    public float MaxHP     { get { return maxHP; } }
    public float CurrentHP
    {
        get { return currentHP; }
        protected set
        {
            currentHP = value;
            UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ChangeHP, this, currentHP / maxHP);
        }
    }
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
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = BGM_Manager.Instance.SeVolume;
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            originalMaterialColor = skinnedMeshRenderer.material.color;

            InitStateMachine();
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }

    void Update()
    {
        stateMachine?.UpdateState();
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            EnemyHitState hitState;

            if (enemy.stateMachine.CurrentState == enemy.stateMachine.GetState(StateName.ENEMY_CHARGE_HIT))
                hitState = enemy.stateMachine.GetState(StateName.ENEMY_CHARGE_HIT) as EnemyChargeHitState;
            else
                hitState = enemy.stateMachine.GetState(StateName.ENEMY_HIT) as EnemyHitState;

            if (enemy.stateMachine.CurrentState == enemy.stateMachine.GetState(StateName.ENEMY_DIE) || hitState.IsHit)
                return;

            ChargingState chargingState = stateMachine.GetState(StateName.CHARGING) as ChargingState;
            var multiplier = weaponManager.Weapon.MultiplierDamage;

            enemy?.Damaged(weaponManager.Weapon.AttackDamage * chargingState.ChargingGauge * multiplier);
        }
    }
    #endregion

    public void OnUpdateStat(float maxHP, float currentHP, float armor, float moveSpeed, int dashCount)
    {
        this.maxHP = maxHP;
        CurrentHP = currentHP;
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
        stateMachine.AddState(StateName.HIT, new HitState());
    }

    public void Damaged(float damage)
    {
        if (stateMachine.CurrentState is DashState || IsDied || stateMachine.CurrentState is HitState)
            return;

        float resultDamage = damage - (armor * 0.01f);
        CurrentHP = Mathf.Clamp((currentHP - resultDamage), 0, maxHP);

        if(Mathf.Approximately(currentHP, 0))
        {
            animator.SetTrigger("Die");
            rigidBody.isKinematic = true;
            IsDied = true;
            StartCoroutine("CorCooldown", ACTIVE_TIME);
            return;
        }

        audioSource.PlayOneShot(hitSound);
        stateMachine.ChangeState(StateName.HIT);
    }

    public void Revive()
    {
        if (IsDied)
        {
            rigidBody.isKinematic = false;
            IsDied = false;
            OnUpdateStat(maxHP, maxHP, armor, moveSpeed, dashCount);
            animator.SetTrigger("Revive");
        }
    }


    public void LoadCurrentHp(float curHp)
    {
        CurrentHP = Mathf.Clamp(curHp, 0, maxHP);

        // CurrentHP가 변경된 시점에 Property에서 호출되어도 괜찮을 듯
        if (Mathf.Approximately(currentHP, 0))
        {
            animator.SetTrigger("Die");
            StartCoroutine("CorCooldown", ACTIVE_TIME);
            IsDied = true;
        }
    }

    public float InitHpBar()
    {
        return currentHP / maxHP;
    }

    IEnumerator CorCooldown(float second)
    {
        float cool = second;
        while (cool > 0)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        FadeInOutController.Instance.FadeOutAndLoadScene("GameOver", StageType.Unknown);
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }
}
