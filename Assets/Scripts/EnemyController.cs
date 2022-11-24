using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
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
        player.TakeDamage();
    }
}
