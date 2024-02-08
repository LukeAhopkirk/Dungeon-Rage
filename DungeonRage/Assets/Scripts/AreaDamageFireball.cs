using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamageFireball : MonoBehaviour
{
    public SkillPointManager skillPointManager;
    public float baseDamage = 35f; // Base damage without multiplier
    public float areaDamageRadius = 3f; // Radius for area damage
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            DealAreaDamage();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            DealAreaDamage();
        }
    }

    private void DealAreaDamage()
    {
        // Access the PlayerMovement script
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerMovement != null)
        {
            // Apply the damage multiplier
            float totalDamage = damage * playerMovement.damageMultiplier;

            // Find all colliders within the area damage radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, areaDamageRadius);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    MonsterController enemy = collider.GetComponent<MonsterController>();

                    // Deal damage to the enemy
                    if (enemy != null)
                    {
                        enemy.TakeDamage(totalDamage);

                        // Access and apply damage to the RageSystem
                        RageSystem rageSystem = FindObjectOfType<RageSystem>();
                        if (rageSystem != null)
                        {
                            rageSystem.DealDamage(totalDamage);
                        }

                        // Show debug log for damage dealt
                        Debug.Log($"AreaDamageFireball dealt {totalDamage} damage to {enemy.gameObject.name}");
                    }
                }
            }
        }

        Destroy(gameObject);
    }
}
