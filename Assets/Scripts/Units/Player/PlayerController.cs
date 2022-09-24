using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{

    #region #�÷��̾� ����
    public enum PlayerState
    {
        MOVE,
        DASH,
        NDASH,
    }

    protected PlayerState playerState;
    #endregion

    #region #������Ʈ
    protected Player player;
    protected Rigidbody rigidBody;
    protected Animator animator;
    protected CapsuleCollider capsuleCollider;
    #endregion

    #region #�̵� ���� ����
    protected const float CONVERT_UNIT_VALUE = 0.01f;
    protected const float DEFAULT_CONVERT_MOVESPEED = 3f;
    protected const float DEFAULT_ANIMATION_PLAYSPEED = 0.9f;
    protected Vector3 inputDirection;          // Ű���� �Է����� ���� �̵� ����
    protected Vector3 calculatedDirection;     // ��� ���� ���� ����� �̵� ����  -> ���� ������ ���� ����
    protected Vector3 gravity;
    #endregion

    #region #��� üũ ����
    [Header("��� ���� �˻�")]
    [SerializeField, Tooltip("ĳ���Ͱ� ����� �� �ִ� �ִ� ��� �����Դϴ�.")]
    float maxSlopeAngle;
    [SerializeField, Tooltip("��� ������ üũ�� Raycast �߻� ���� �����Դϴ�.")]
    Transform raycastOrigin;

    private const float RAY_DISTANCE = 2f;
    private RaycastHit slopeHit;
    private bool isOnSlope;
    #endregion

    #region #�ٴ� üũ ����
    [Header("�� üũ")]
    [SerializeField, Tooltip("ĳ���Ͱ� ���� �پ� �ִ��� Ȯ���ϱ� ���� CheckBox ���� �����Դϴ�.")]
    Transform groundCheck;
    private int groundLayer;
    private bool isGrounded;
    #endregion

    #region #��� ����
    [Header("���(Dash) �ɼ�")]
    [Header("����) DashForwardRollTime + DashReInputTime = 0.4�ʸ� ���Ѿ� �մϴ�.")]
    [SerializeField, Tooltip("���(Dash)�� ���� ��Ÿ���� ���Դϴ�.")]
    protected float dashPower;
    [SerializeField, Tooltip("���(Dash) �ձ����� ��� �ð�")]
    protected float dashForwardRollTime;
    [SerializeField, Tooltip("���(Dash) ���� ��, �ٽ� ��� �Է��� ���� �� �ִ� �ð�")]
    protected float dashReInputTime;
    [SerializeField, Tooltip("���(Dash) ��, ���� �ð�")]
    protected float dashTetanyTime;
    [SerializeField, Tooltip("���(Dash) ���� ���ð�")]
    protected float dashCoolTime;

    private WaitForSeconds DASH_FORWARD_ROLL_TIME;
    private WaitForSeconds DASH_RE_INPUT_TIME;
    private WaitForSeconds DASH_TETANY_TIME;
    private Coroutine dashCoroutine;
    private Coroutine dashCoolTimeCoroutine;
    private int currentDashCount;
    #endregion

    #region #UNITY_FUNCTIONS
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        DASH_FORWARD_ROLL_TIME = new WaitForSeconds(dashForwardRollTime);
        DASH_RE_INPUT_TIME = new WaitForSeconds(dashReInputTime);
        DASH_TETANY_TIME = new WaitForSeconds(dashTetanyTime);
        playerState = PlayerState.MOVE;
    }

    void Update()
    {
        calculatedDirection = GetDirection(player.MoveSpeed * CONVERT_UNIT_VALUE);
        ControlGravity();
    }

    void FixedUpdate()
    {
        Move(calculatedDirection, player.MoveSpeed * CONVERT_UNIT_VALUE);
    }

    #endregion


    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool isAvailableDash = playerState != PlayerState.DASH && currentDashCount < player.DashCount && isGrounded;

            if (isAvailableDash)
            {
                playerState = PlayerState.DASH;
                currentDashCount++;

                if (dashCoroutine != null && dashCoolTimeCoroutine != null)
                {
                    StopCoroutine(dashCoroutine);
                    StopCoroutine(dashCoolTimeCoroutine);
                }

                dashCoroutine = StartCoroutine(DashCoroutine());
            }
        }
    }

    private IEnumerator DashCoroutine()
    {
        Vector3 LookAtDirection = (inputDirection == Vector3.zero) ? transform.forward : inputDirection;
        Vector3 dashDirection = (calculatedDirection == Vector3.zero) ? transform.forward : calculatedDirection;

        animator.SetFloat("Velocity", 0f);
        animator.SetBool("IsDashing", true);
        animator.SetTrigger("Dash");
        LookAt(LookAtDirection);
        rigidBody.velocity = dashDirection * dashPower;

        yield return DASH_FORWARD_ROLL_TIME;       // ��� �ձ����� ��� �ð�
        playerState = (player.DashCount > 1 && currentDashCount < player.DashCount) ? PlayerState.NDASH : PlayerState.DASH;
        
        yield return DASH_RE_INPUT_TIME;           // N�� ��ð� ������ ��, Ű �Է��� ���� �� �ִ� �ð�
        animator.SetBool("IsDashing", false);
        rigidBody.velocity = Vector3.zero;

        yield return DASH_TETANY_TIME;             // ��� ��, ���� �ð�
        playerState = PlayerState.MOVE;

        dashCoolTimeCoroutine = StartCoroutine(DashCoolTimeCoroutine());   // ��� ��Ÿ�� üũ ����
    }

    private IEnumerator DashCoolTimeCoroutine()
    {
        float currentTime = 0f;
        while (true)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= dashCoolTime)
                break;
            yield return null;
        }

        if (currentDashCount == player.DashCount)
            currentDashCount = 0;
    }

    protected Vector3 GetDirection(float currentMoveSpeed)
    {
        isOnSlope = IsOnSlope();
        isGrounded = IsGrounded();
        Vector3 calculatedDirection = CalculateNextFrameGroundAngle(currentMoveSpeed) < maxSlopeAngle ? inputDirection : Vector3.zero;

        calculatedDirection = (isGrounded && isOnSlope) ? AdjustDirectionToSlope(calculatedDirection) : calculatedDirection.normalized;
        return calculatedDirection;
    }

    protected void ControlGravity()
    {
        gravity = Vector3.down * Mathf.Abs(rigidBody.velocity.y);

        if (isGrounded && isOnSlope)
        {
            gravity = Vector3.zero;
            rigidBody.useGravity = false;
            return;
        }
        rigidBody.useGravity = true;
    }

    protected void Move(Vector3 moveDirection, float currentMoveSpeed)
    {
        if (playerState != PlayerState.MOVE)
            return;

        float animationPlaySpeed = DEFAULT_ANIMATION_PLAYSPEED + GetAnimationSyncWithMovement(currentMoveSpeed);
        LookAt(inputDirection);
        rigidBody.velocity = moveDirection * currentMoveSpeed + gravity;
        animator.SetFloat("Velocity", animationPlaySpeed);
    }

    private float CalculateNextFrameGroundAngle(float moveSpeed)
    {
        var nextFramePlayerPosition = raycastOrigin.position + inputDirection * moveSpeed * Time.fixedDeltaTime;   // ���� ������ ĳ���� �� �κ� ��ġ

        if (Physics.Raycast(nextFramePlayerPosition, Vector3.down, out RaycastHit hitInfo, RAY_DISTANCE, groundLayer))
        {
            return Vector3.Angle(Vector3.up, hitInfo.normal);
        }
        return 0f;
    }

    public bool IsGrounded()
    {
        Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.4f, transform.lossyScale.z);
        return Physics.CheckBox(groundCheck.position, boxSize, Quaternion.identity, groundLayer);
    }

    //private void OnDrawGizmos()
    //{
    //    Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.4f, transform.lossyScale.z);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(groundCheck.position, boxSize);

    //    Ray ray = new Ray(transform.position, Vector3.down);
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawRay(ray);
    //}

    public bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(ray, out slopeHit, RAY_DISTANCE, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlopeAngle;
        }
        return false;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        inputDirection = new Vector3(input.x, 0f, input.y);
    }

    protected void LookAt(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(direction);
            rigidBody.rotation = targetAngle;
        }
    }

    protected float GetAnimationSyncWithMovement(float changedMoveSpeed)
    {
        if (inputDirection == Vector3.zero)
        {
            return -DEFAULT_ANIMATION_PLAYSPEED;
        }

        // (�ٲ� �̵� �ӵ� - �⺻ �̵��ӵ�) * 0.1f
        return (changedMoveSpeed - DEFAULT_CONVERT_MOVESPEED) * 0.1f;
    }

    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        Vector3 adjustVelocityDirection = Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        return adjustVelocityDirection;
    }
}
