using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TokenInstance : MonoBehaviour
{
    public bool collected = false;

    private GameController controller;

    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void OnTriggerEnter(Collider other)
    {
        //only exectue OnPlayerEnter if the player collides with this token.
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null) OnPlayerEnter(player);
    }

    void OnPlayerEnter(PlayerController player)
    {
        if (collected) return;
        //disable the gameObject and remove it from the controller update list.
        if (controller != null)
            collected = true;
        //send an event into the gameplay system to perform some behaviour.
        controller.CollectToken(this);
    }
}
