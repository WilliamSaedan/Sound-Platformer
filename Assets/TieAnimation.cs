using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TieAnimation : MonoBehaviour
{
    public PlayerController pc;

    public void beat()
    {
        SendMessageUpwards("Beat");
    }
}
