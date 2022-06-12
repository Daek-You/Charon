using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CMManager : MonoBehaviour
{
    public GameObject sondol;
    public CinemachineVirtualCamera vCam1 = null;
    public CinemachineVirtualCamera vCam2 = null;

    const float doorStg1_4Start = 1000.0f;
    const float doorStg1_4End = 2000.0f;
    const float doorStg1_5Start = 2400.0f;
    const float doorStg1_5End = 3500.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OnChangePriority();
    }

    private void OnChangePriority()
    {
        if (sondol.transform.position.x * sondol.transform.position.z < doorStg1_4Start)
        {
            vCam1.Priority = 11;
            vCam2.Priority = 9;
        }

        else if (sondol.transform.position.x * sondol.transform.position.z >= doorStg1_4Start && sondol.transform.position.x * sondol.transform.position.z < doorStg1_4End)
        {
            vCam1.Priority = 9;
            vCam2.Priority = 11;
        }

        else if (sondol.transform.position.x * sondol.transform.position.z >= doorStg1_4End && sondol.transform.position.x * sondol.transform.position.z < doorStg1_5Start)
        {
            vCam1.Priority = 11;
            vCam2.Priority = 9;
        }

        else if (sondol.transform.position.x * sondol.transform.position.z >= doorStg1_5Start && sondol.transform.position.x * sondol.transform.position.z < doorStg1_5End)
        {
            vCam1.Priority = 9;
            vCam2.Priority = 11;

        }
        else if (sondol.transform.position.x * sondol.transform.position.z > doorStg1_5End)
        {
            vCam1.Priority = 11;
            vCam2.Priority = 9;
        }

    }


}
