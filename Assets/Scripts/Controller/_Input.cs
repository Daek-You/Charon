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
            if (!owner.isDash || owner.canInputKey)
            {
                DashVecter = MoveVelocity;
                ++CurrentDashCount;
                owner.isDash = true;
                owner.canInputKey = false;
                owner.thePhysics.DashMove(owner);
            }
        }
    }

    private void AttackInput(Controller owner)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (owner.isDash && owner.canInputKey)
            {
                Vector3 mouseDirection = GetMouseWorldDirectionOrZero();
                Coroutine dashCor = owner.thePhysics.dashCoroutine;
                
                if(dashCor != null)
                    GetComponent<_Physics>().StopCoroutine(dashCor);
                
                owner.sondol.myWeapon.DashAttack(owner, mouseDirection);
                return;
            }

            StartCoroutine(ChargingCheckCor(owner));
        }
    }


    private Vector3 GetMouseWorldDirectionOrZero()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 target = hit.point;
            target.Set(target.x, 0f, target.z);
            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 5f);
            return (target - transform.position).normalized;
        }

        return Vector3.zero;
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
