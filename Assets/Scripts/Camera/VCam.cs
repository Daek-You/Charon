using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCam : MonoBehaviour
{
    public static VCam Instance                 { get; private set; }
    public CinemachineVirtualCamera virtualCam  { get; private set; }
    public Camera mainCam;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            mainCam = Camera.main;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(mainCam);
            return;
        }
        DestroyImmediate(gameObject);
        DestroyImmediate(mainCam);
    }

    void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        
    }
}
