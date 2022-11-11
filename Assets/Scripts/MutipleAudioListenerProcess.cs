using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MutipleAudioListenerProcess : MonoBehaviour
{
    public Camera cam;


    void Start()
    {
        GameObject sondol = GameObject.Find("Sondol");

        if(sondol == null)
        {
            cam.AddComponent<AudioListener>();
        }
    }
}
