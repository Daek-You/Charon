using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOtherTrigger : MonoBehaviour
{
    [SerializeField]
    private StageType wallType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Sondol"))
            return;

        // 트리거 오브젝트가 이벤트를 호출할 경우 다음 스테이지를 실행
        StageManager.Instance.ActiveStage(wallType + 1);
        gameObject.SetActive(false);
    }
}
