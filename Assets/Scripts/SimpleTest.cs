using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTest : MonoBehaviour
{
    private GameObject osc;

    void Awake()
    {
        osc = GameObject.FindWithTag("OSC");
    }

    public void OnButtonPress()
    {
        osc.GetComponent<OSCSendReceive>().PlaySoundOSC("/Simple_Test " + 4567);
        osc.GetComponent<OSCSendReceive>().PlaySoundOSC("/test_tag " + 498 + " 1");
    }
}
