using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PdReceive : MonoBehaviour
{
    //The Pure Data patch we will listen to.
    public LibPdInstance pdPatch;

    //Setup the Component to listen to the specified send object when the
    //Component is created.
    void Start()
    {
        pdPatch.Bind("SimpleTest");
    }

    public void UpdateRoundStart()
    {
        pdPatch.SendBang("Test_Signal");
    }
}