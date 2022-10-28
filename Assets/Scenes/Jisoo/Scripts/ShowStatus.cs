using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowStatus : MonoBehaviour
{
    public TMP_Text[] status = new TMP_Text[4];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        status[0].text = StatManager.Instance.currentMaxHP.ToString();
        status[1].text = StatManager.Instance.currentArmor.ToString();
        status[2].text = StatManager.Instance.currentMoveSpeed.ToString();
        status[3].text = StatManager.Instance.currentDashCount.ToString();
    }
}
