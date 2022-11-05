using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCameraTest : MonoBehaviour
{
    public GameObject target;
    public float offsetX;
    public float offsetY;
    public float offsetZ;

    private void Update()
    {
        Vector3 targetPos = target.transform.position;
        Vector3 fixedPos = new Vector3(targetPos.x + offsetX, targetPos.y + offsetY, targetPos.z + offsetZ);
        transform.position = fixedPos;

        if (Input.GetKeyDown(KeyCode.Tab))
            target.transform.position = new Vector3(0, 0, 0);
    }
}
