using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int baseDamage = 30; // Base damage without multiplier

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            MonsterController enemy = collision.gameObject.GetComponent<MonsterController>();

            if (enemy != null)
            {
                // Access the PlayerMovement script
                PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

                if (playerMovement != null)
                {
                    // Apply the damage multiplier
                    int totalDamage = Mathf.RoundToInt(baseDamage * playerMovement.damageMultiplier);

                    // Deal damage to the enemy
                    enemy.TakeDamage(totalDamage);

                    // Access and apply damage to the RageSystem
                    RageSystem rageSystem = FindObjectOfType<RageSystem>();
                    if (rageSystem != null)
                    {
                        rageSystem.DealDamage(totalDamage);
                    }

                    // Show debug log for damage dealt
                    Debug.Log($"Fireball dealt {totalDamage} damage to {enemy.gameObject.name}");
                }
            }

            Destroy(gameObject);
        }
    }
}
