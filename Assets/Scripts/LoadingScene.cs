using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{

    float timer = 0f;
    private void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 4f)
        {
            SceneManager.LoadScene("LobbyScene");
        }
        
    }
}
