using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        MOVE,
        DASH,
    }

    protected PlayerState playerState;

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
    [SerializeField, Tooltip("���(Dash)�� ���� ��Ÿ���� ���Դϴ�.")]
    protected float dashPower;
    [SerializeField, Tooltip("���(Dash)�� ���ӽð��� ��Ÿ���� ���Դϴ�.")]
    protected float dashDuration;
    private WaitForSeconds DASH_DURATION;
    #endregion

    #region #UNITY_FUNCTIONS
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        DASH_DURATION = new WaitForSeconds(dashDuration);
        playerState = PlayerState.MOVE;
    }

    void FixedUpdate()
    {
        float currentMoveSpeed = player.MoveSpeed * CONVERT_UNIT_VALUE;
        calculatedDirection = GetDirection(currentMoveSpeed);
        //Debug.Log(calculatedDirection);
        ControlGravity();
        Move(calculatedDirection, currentMoveSpeed);
    }

    #endregion

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed && playerState != PlayerState.DASH)
        {
            playerState = PlayerState.DASH;
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        Vector3 dashDirection = (calculatedDirection == Vector3.zero) ? transform.forward : calculatedDirection;
        dashDirection = (isGrounded && isOnSlope) ? AdjustDirectionToSlope(dashDirection) : dashDirection;

        animator.SetFloat("Velocity", 0f);
        animator.SetTrigger("Dash");
        rigidBody.velocity = dashDirection * dashPower;

        yield return DASH_DURATION;
        playerState = PlayerState.MOVE;
        rigidBody.velocity = Vector3.zero;

    }

    protected Vector3 GetDirection(float currentMoveSpeed)
    {
        isOnSlope = IsOnSlope();
        isGrounded = IsGrounded();
        Vector3 calculatedDirection = CalculateNextFrameGroundAngle(currentMoveSpeed) < maxSlopeAngle ? inputDirection : Vector3.zero;

        calculatedDirection = (isGrounded && isOnSlope) ? AdjustDirectionToSlope(calculatedDirection) : calculatedDirection;
        return calculatedDirection;
    }

    protected void ControlGravity()
    {
        gravity = Vector3.down * Mathf.Abs(rigidBody.velocity.y);

        if(isGrounded && isOnSlope)
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
        LookAt();
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

    private void OnDrawGizmos()
    {
        Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.4f, transform.lossyScale.z);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, boxSize);

        Ray ray = new Ray(transform.position, Vector3.down);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(ray);
    }

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

    protected void LookAt()
    {
        if (inputDirection != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(inputDirection);
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
