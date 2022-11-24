using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth { get; set; }
    public bool IsAlive => currentHealth > 0;
    public bool invincible { get; set; }

    public void Increment()
    {
        currentHealth = Mathf.Clamp(currentHealth + 1, 0, maxHealth);
    }

    /// <summary>
    /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
    /// current HP reaches 0.
    /// </summary>
    public void Decrement()
    {
        currentHealth = Mathf.Clamp(currentHealth - 1, 0, maxHealth);
        if (currentHealth == 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Decrement the HP of the entitiy until HP reaches 0.
    /// </summary>
    public void Die()
    {
        currentHealth = 0;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }


}
