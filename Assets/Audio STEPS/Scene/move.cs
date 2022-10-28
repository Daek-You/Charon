using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class move : MonoBehaviour
{
    public float vel = .05f;
    public AudioSource Step;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x_dir = Input.GetAxis("Horizontal");
        float y_dir = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x_dir, 0.0f, y_dir);
        transform.position += move * vel;
        
        
        Step.Play();

        Thread.Sleep(60);

    }
}
