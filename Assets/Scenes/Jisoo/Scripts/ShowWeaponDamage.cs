using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowWeaponDamage : MonoBehaviour
{
    public TMP_Text[] status = new TMP_Text[3];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //status[0].text = StatManager.Instance.currentPaddleDamage.ToString();
        //status[1].text = StatManager.Instance.currentSickleDamage.ToString();
        //status[2].text = StatManager.Instance.currentGourdDamage.ToString();
    }
}
