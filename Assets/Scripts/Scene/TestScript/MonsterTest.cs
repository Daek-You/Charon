using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTest : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onDead;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            onDead.Invoke();
            gameObject.SetActive(false);
        }
    }
}
