using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTest : MonoBehaviour
{
    [SerializeField]
    private StageType wallType;

    private void Start()
    {
        Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Sondol"))
            return;

        // 스테이지 오브젝트 Trigger가 반응할 경우 다음 스테이지를 실행
        // 다음 스테이지가 없을 경우?
        Debug.Log("다음 스테이지 실행");

        Utils.Destroy(gameObject, Time.deltaTime);
    }

    private void Init()
    {
        UIManager.EventHandler.AddListener(UI_EventHandler.UIEventType.ChangeClear, OnCheckClear);
    }

    public void OnCheckClear(UI_EventHandler.UIEventType eventType, Component sender, object param = null)
    {
        if ((bool)param != true)
            return;
        if (StageManager.Instance.CurrentStage != wallType)
            return;

        // 사용자에게 시각적으로 통행 가능하다는 알림을 주기 위해 Mesh Renderer를 비활성화함
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        // 사용자가 해당 오브젝트를 지나갈 수 있도록 IsTriiger를 True로 변경함
        gameObject.GetComponent<Collider>().isTrigger = true;
    }
}
