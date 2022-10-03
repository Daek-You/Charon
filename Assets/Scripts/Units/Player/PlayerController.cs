using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using CharacterController;


[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    public Vector3 MouseDirection { get; private set; }
    public Player player { get; private set; }
    public Vector3 inputDirection { get; private set; }          // Ű���� �Է����� ���� �̵� ����
    public Vector3 calculatedDirection { get; private set; }     // ��� ���� ���� ����� �̵� ����
    public Vector3 gravity { get; private set; }

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
        player = GetComponent<Player>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    void Update()
    {
        calculatedDirection = GetDirection(player.MoveSpeed * MoveState.CONVERT_UNIT_VALUE);
        ControlGravity();
    }
    #endregion


    public void OnClickLeftMouse(InputAction.CallbackContext context)
    {
        /// �� ���ε��� ���� ��ȣ�ۿ��� ���� ���,
        /// ��ȣ�ۿ� �켱������ ��ȣ�ۿ뿡 �߰��ߴ� ��������̸�
        /// ��ȣ�ۿ��� �߰��� ��ҵǾ�� ���� ��ȣ�ۿ��� �ߵ��� ������ ��� �ȴ�.
        ///   - �� ���� ��ȣ�ۿ��� ���������� ����(performed)�ƴٸ�, ���� ��ȣ�ۿ��� ������� ����


        if (context.performed)
        {
            MouseDirection = GetMouseWorldPosition();

            if (context.interaction is HoldInteraction)         // ���� ����
            {
                LookAt(MouseDirection);
                /// ���� ���� ���� ��ȯ
            }

            else if (context.interaction is PressInteraction)   // �Ϲ� ����
            {
                if (DashState.IsDash)  // �ϴ��� ���Ƴ���
                {
                    /// ��� ���� ������ȯ
                    return;
                }

                bool isAvailableAttack = !AttackState.IsAttack && (player.weaponManager.Weapon.ComboCount < 3);
                if (isAvailableAttack)
                {
                    LookAt(MouseDirection);
                    player.stateMachine.ChangeState(StateName.ATTACK);
                }
            }
        }
    }


    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            bool isAvailableDash = DashState.CanOtherBehaviour && DashState.CurrentDashCount < player.DashCount && isGrounded;

            if (isAvailableDash && !AttackState.IsAttack)
            {
                player.stateMachine.ChangeState(StateName.DASH);
            }
        }
    }

    protected Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 5f);

        if (Physics.Raycast(ray, out RaycastHit HitInfo, Mathf.Infinity))
        {
            Vector3 target = HitInfo.point;
            Vector3 myPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            target.Set(target.x, 0f, target.z);
            return (target - myPosition).normalized;
        }
        return Vector3.zero;
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
        gravity = Vector3.down * Mathf.Abs(player.rigidBody.velocity.y);

        if (isGrounded && isOnSlope)
        {
            gravity = Vector3.zero;
            player.rigidBody.useGravity = false;
            return;
        }
        player.rigidBody.useGravity = true;
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
        if (Physics.Raycast(ray, out slopeHit, RAY_DISTANCE, groundLayer))
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
            transform.rotation = targetAngle;
        }
    }

    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        Vector3 adjustVelocityDirection = Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        return adjustVelocityDirection;
    }
}
