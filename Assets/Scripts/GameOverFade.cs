using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFade : MonoBehaviour
{
    private float ACTIVE_TIME = 5.0f;

    private void Start()
    {
        Player.Instance.Revive();
        DataManager.Instance.IsDirOrClear = true;
        StartCoroutine("CorCooldown", ACTIVE_TIME);
    }

    IEnumerator CorCooldown(float second)
    {
        float cool = second;
        while (cool > 0)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        FadeInOutController.Instance.FadeOutAndLoadScene("LobbyScene", StageType.Lobby);
    }
}
