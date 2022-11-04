using CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public float CurrentCoolTime { get; private set; } = 0f;
    public Dictionary<string, IEffect> myWeaponEffects { get; private set; }
    public Dictionary<string, ISound> mySounds { get; private set; }
    private Color originColor;                           /// ��Ÿ�� �ð��� �׽�Ʈ ����
    private DashState dashState;
    private DashAttackState dashAttackState;
    private ChargingAttackState chargingAttackState;
    private AttackState attackState;
    private SkillState skillState;
    
    private Coroutine dashCoolTimeCoroutine;
    private SkinnedMeshRenderer skinnedMeshRenderer;     /// ��Ÿ�� �ð��� �׽�Ʈ ����


    #region #Unity Functions
    void Awake()
    {
        myWeaponEffects = new Dictionary<string, IEffect>();
        mySounds = new Dictionary<string, ISound>();
    }

    void Start()
    {
        dashState = Player.Instance.stateMachine.GetState(StateName.DASH) as DashState;
        dashAttackState = Player.Instance.stateMachine.GetState(StateName.DASH_ATTACK) as DashAttackState;
        attackState = Player.Instance.stateMachine.GetState(StateName.ATTACK) as AttackState;
        chargingAttackState = Player.Instance.stateMachine.GetState(StateName.CHARGING_ATTACK) as ChargingAttackState;
        skillState = Player.Instance.stateMachine.GetState(StateName.SKILL) as SkillState;
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = skinnedMeshRenderer.material.color;
    }

    public void OnParticleCollision(GameObject other)
    {
        
    }
    void Update()
    {
        if (attackState.IsAttack)
        {
            Vector3 velocity = Player.Instance.rigidBody.velocity;
            Player.Instance.rigidBody.velocity = new Vector3(velocity.x, 0f, velocity.z);
        }
    }
    #endregion

    public void OnStartSkill()
    {
        if (myWeaponEffects.TryGetValue(Player.Instance.weaponManager.Weapon.Name, out IEffect weapon))
        {
            weapon.PlaySkillEffect();
        }

        if (mySounds.TryGetValue(Player.Instance.weaponManager.Weapon.Name, out ISound weaponSound))
        {
            weaponSound.PlaySkillSound();
        }
    }

    public void OnFinishedSkill()
    {
        skillState.IsSkillActive = false;
        Player.Instance.animator.SetBool("IsSkill", false);
        Player.Instance.weaponManager.Weapon.CurrentSkillGauge = 0;
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnDestroyEffect()
    {
        if (myWeaponEffects.TryGetValue(Player.Instance.weaponManager.Weapon.Name, out IEffect weapon))
        {
            weapon.DestroyEffect();
        }
    }

    public void OnStartChargingAttack()
    {
        if (myWeaponEffects.TryGetValue(Player.Instance.weaponManager.Weapon.Name, out IEffect weapon))
        {
            weapon.PlayChargingAttackEffect();
        }

        if (mySounds.TryGetValue(Player.Instance.weaponManager.Weapon.Name, out ISound weaponSound))
        {
            weaponSound.PlayChargingAttackSound();
        }
    }

    public void OnFinishedChargingAttack()
    {
        chargingAttackState.IsChargingAttack = false;
        Player.Instance.animator.SetBool("IsCharingAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnPlayFootStepSound()
    {
        if (Player.Instance.Controller.isGrounded)
        {
            bool currentStep = Player.Instance.Controller.IsFirstStep;
            Player.Instance.Controller.IsFirstStep = !currentStep;

            int clipIndex = !currentStep ? 0 : 1;
            AudioClip clip = Player.Instance.Controller.footstepSounds[clipIndex];
            Player.Instance.audioSource.PlayOneShot(clip);
        }
    }

    public void OnStartAttack()
    {

        if(myWeaponEffects.TryGetValue(Player.Instance.weaponManager.Weapon.Name, out IEffect weapon))
        {
            weapon.PlayComboAttackEffects();
        }
        
        if(mySounds.TryGetValue(Player.Instance.weaponManager.Weapon.Name, out ISound weaponSound))
        {
            weaponSound.PlayComboAttackSound();
        }
    }

    public void OnStartDashAttack()
    {
        if (myWeaponEffects.TryGetValue(Player.Instance.weaponManager.Weapon.Name, out IEffect weapon))
        {
            weapon.PlayDashAttackEffect();
        }

        if (mySounds.TryGetValue(Player.Instance.weaponManager.Weapon.Name, out ISound weaponSound))
        {
            weaponSound.PlayDashAttackSound();
        }
    }

    public void OnFinishedAttack()
    {
        attackState.IsAttack = false;
        Player.Instance.animator.SetBool("IsAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }
    
    public void OnFinishedDashAttack()
    {
        dashAttackState.IsDashAttack = false;
        dashState.CanDashAttack = false;
        Player.Instance.animator.SetBool("IsDashAttack", false);
        Player.Instance.stateMachine.ChangeState(StateName.MOVE);
    }

    public void OnCanDashAttack()
    {
        dashState.CanDashAttack = true;
    }

    public void OnChangeDashToDashAttack()
    {
        if (dashAttackState.IsPressDashAttack)
        {
            dashAttackState.IsPressDashAttack = false;
            dashState.inputDirectionBuffer.Clear();
            Player.Instance.stateMachine.ChangeState(StateName.DASH_ATTACK);
        }
    }

    public void OnFinishedDash()
    {
        if (!dashAttackState.IsDashAttack)
        {
            dashState.CanDashAttack = false;

            if (dashState.inputDirectionBuffer.Count > 0)
            {
                Player.Instance.stateMachine.ChangeState(StateName.DASH);
                return;
            }

            dashState.CanAddInputBuffer = false;
            dashState.OnExitState();

            if (dashCoolTimeCoroutine != null)
                StopCoroutine(dashCoolTimeCoroutine);
            dashCoolTimeCoroutine = StartCoroutine(CheckDashReInputLimitTime(dashState.dashCooltime));
        }
    }

    public void OnCanAddToDashInputBuffer()
    {
        dashState.CanAddInputBuffer = true;
    }

    private IEnumerator CheckDashReInputLimitTime(float limitTime)
    {
        float timer = 0f;
        skinnedMeshRenderer.material.color = Color.red;

        while (true)
        {
            timer += Time.deltaTime;
            
            if(timer > limitTime)
            {
                dashState.IsDash = false;
                dashState.CurrentDashCount = 0;
                skinnedMeshRenderer.material.color = originColor;
                Player.Instance.stateMachine.ChangeState(StateName.MOVE);
                break;
            }
            yield return null;
        }
    }
}
