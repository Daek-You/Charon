using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class _Input : MonoBehaviour, IComponent<Controller>
{

    public static bool Enable = true;
    public Vector3 MoveVelocity { get; private set; }
    public Vector3 DashVecter { get; private set; }
    public int CurrentDashCount { get; set; } = 0;


    private const float CHARGING_THRESHOLD = 0.1f;


    public void UpdateComponent(Controller owner)
    {
        if (Enable)
        {
            UpdateVelocity();
            UpdateDashVector(owner);
            AttackInput(owner);
        }

        else
        {
            MoveVelocity = DashVecter = Vector3.zero;
            owner.isCharging = false;
        }

    }

    private void UpdateVelocity()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool hasControl = !Mathf.Approximately(horizontal, 0f) || !Mathf.Approximately(vertical, 0f);
        MoveVelocity = hasControl ? new Vector3(horizontal, 0f, vertical).normalized : Vector3.zero;
    }

    private void UpdateDashVector(Controller owner)
    {
        if (Input.GetKeyDown(KeyCode.Space) && CurrentDashCount < owner.sondol.dashCount)
        {
            if (!owner.isDash)
            {
                DashVecter = MoveVelocity;
                ++CurrentDashCount;
                owner.isDash = true;
                owner.thePhysics.DashMove(owner);
            }
        }
    }

    private void AttackInput(Controller owner)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (owner.canDashAttack)
            {
                /// �뽬 ���� ó��
                Debug.Log("�뽬 ����!");
                return;
            }

            StartCoroutine(ChargingCheckCor(owner));
        }
    }


    private IEnumerator ChargingCheckCor(Controller owner)
    {
        owner.isCharging = false;
        float startTime = Time.time;
        float chargingTime = 0f;

        while (!Input.GetMouseButtonUp(0))               // ���콺 ��Ŭ�� ��, ���� ���� �ʾҴٸ�
        {
            bool isChargeStart = (Time.time > startTime + CHARGING_THRESHOLD) && !owner.isCharging;   // ��¡�� �Ѵٰ� �����Ǵ� �ð��� �Ǿ��°�?

            if (isChargeStart)                           // ���� �������� ��¡ �ð� ���
                owner.isCharging = true;

            if (owner.isCharging && chargingTime < 3f)         // ��¡ �ð� ���
            {
                chargingTime += Time.deltaTime;
                Debug.Log($"��¡ ��...");
            }         

            else if (owner.isCharging && chargingTime >= 3f)   // 3�� �̻� ��¡�� ����
            {
                /// ��� ���
                Debug.Log($"��¡ �ð� �ʰ� ����");
                yield break;
            }

            yield return null;
        }


        if (owner.isCharging)
        {
            /// ��¡ ����
            Debug.Log("��¡ ����!");
            yield break;
        }

        Debug.Log("�Ϲ� 3Ÿ ����");
        /// 3Ÿ ���� ����
    }
}
