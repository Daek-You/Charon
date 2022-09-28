using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CharacterController;


[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public StateMachine stateMachine { get; private set; }

    #region #������Ʈ
    public Player player { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public Animator animator { get; private set; }
    public CapsuleCollider capsuleCollider { get; private set; }
    #endregion

    #region #���� �� �߷� ����
    public Vector3 inputDirection { get; private set; }          // Ű���� �Է����� ���� �̵� ����
    public Vector3 calculatedDirection { get; private set; }     // ��� ���� ���� ����� �̵� ����  -> ���� ������ ���� ����
    public Vector3 gravity { get; private set; }
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

    #region #UNITY_FUNCTIONS
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");

        stateMachine = new StateMachine();
        stateMachine.AddState(StateName.MOVE, new MoveState(this));
        stateMachine.AddState(StateName.DASH, new DashState(this));
        stateMachine.ChangeState(StateName.MOVE);
    }

    void Update()
    {
        calculatedDirection = GetDirection(player.MoveSpeed * MoveState.CONVERT_UNIT_VALUE);
        ControlGravity();
        stateMachine?.UpdateState();
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }
    #endregion

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool isAvailableDash = !(stateMachine.currentState is DashState) &&
                                    DashState.currentDashCount < player.DashCount && isGrounded;

            if (isAvailableDash)
            {
                stateMachine.ChangeState(StateName.DASH);
            }
        }
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

    private float CalculateNextFrameGroundAngle(float moveSpeed)
    {
        // ���� ������ ĳ���� �� �κ� ��ġ
        Vector3 nextFramePlayerPosition = raycastOrigin.position + inputDirection * moveSpeed * Time.fixedDeltaTime;   

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

    public void LookAt(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(direction);
            rigidBody.rotation = targetAngle;
        }
    }

    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        Vector3 adjustVelocityDirection = Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        return adjustVelocityDirection;
    }
}
