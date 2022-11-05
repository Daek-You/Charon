using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private LayerMask interactableLayermask = 11;
    private RaycastHit[] hits;

    [SerializeField]
    private float interactRadius = 2;
    private int order;
    private Interactable interObj;

    private void Update()
    {
        hits = Physics.SphereCastAll(transform.position, interactRadius, transform.forward, 0, interactableLayermask);
        if (hits.Length < 1 || UIManager.Instance.IsPopupOpened)
        {
            UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.HideUI, this);
            return;
        }

        if (hits.Length == 1)
            order = 0;
        if (hits.Length > 1)
            order = CalculateDistance();

        interObj = hits[order].collider.GetComponent<Interactable>();
        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.HideUI, this, interObj.name);
        UIManager.EventHandler.PostNotification(UI_EventHandler.UIEventType.ShowUI, this, interObj.name);

        // 상호작용 버튼을 눌러서 기능 실행
        if (Input.GetKeyDown(KeyCode.F))
            interObj.interact();
    }

    private int CalculateDistance()
    {
        int num = 0;
        float minDistance = (float)interactRadius + 1;

        for (int i = 0; i < hits.Length; i++)
        {
            if (i == 0)
            {
                minDistance = Vector3.Distance(transform.position, hits[i].collider.transform.position);
                continue;
            }
            
            float distance = Vector3.Distance(transform.position, hits[i].collider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                num = i;
            }
        }

        return num;
    }
}
