using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack
{
    public Unit unit;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("�浹����");
        }
        else
            Debug.Log("����");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
