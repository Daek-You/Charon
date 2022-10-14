using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverEvent : MonoBehaviour
{
    QuestReporter reporter;

    private void Start()
    {
        Init();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Sondol"))
            return;

        reporter.Report();
    }

    private void Init()
    {
        reporter = Utils.GetAddedComponent<QuestReporter>(this.gameObject);
    }
}
