using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class _Input : MonoBehaviour, IComponent<Controller>
{

    public static bool Enable = true;
    public Vector3 MoveVelocity { get; private set; }
    public Vector3 DashVecter { get; private set; }
    public int CurrentDashCount { get; set; } = 0;


    public void UpdateComponent(Controller owner)
    {
        if (Enable)
        {
            UpdateVelocity();
            UpdateDashVector(owner);
        }

        else
        {
            MoveVelocity = DashVecter = Vector3.zero;
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
}
