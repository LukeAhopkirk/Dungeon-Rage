using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public SkillPointManager skillPointManager;
    public float baseDamage = 35f; // Base damage without multiplier
    public float damage;
    public GameObject hitParticlesPrefab; // Reference to the Particle System prefab

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            DestroyFireball();
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Tank") || collision.gameObject.CompareTag("Range"))
        {
            // Access the PlayerMovement script
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();

            // Check if the enemy has the MonsterController component
            MonsterController monsterController = collision.gameObject.GetComponent<MonsterController>();
            // Check if the enemy has the TankController component
            TankController tankController = collision.gameObject.GetComponent<TankController>();
            // Check if the enemy has the RangeController component
            RangeController rangeController = collision.gameObject.GetComponent<RangeController>();

            float totalDamage = 0;

            if (playerMovement != null)
            {
                // Apply the damage multiplier
                totalDamage = damage * playerMovement.damageMultiplier;

                // Access and apply damage to the RageSystem
                RageSystem rageSystem = FindObjectOfType<RageSystem>();
                if (rageSystem != null)
                {
                    rageSystem.DealDamage(totalDamage);
                }

            }

            // If it's a basic monster
            if (monsterController != null)
            {
                monsterController.TakeDamage(totalDamage);
            }
            // If it's a tank enemy
            else if (tankController != null)
            {
                tankController.TakeDamage(totalDamage);
            }
            // If it's a ranged enemy
            else if (rangeController != null)
            {
                rangeController.TakeDamage(totalDamage);
            }

            Debug.Log($"Total damage {totalDamage}");

            // Spawn hit particles at the collision point
            SpawnHitParticles(transform.position);

            // Destroy the fireball
            DestroyFireball();
        }
    }

    void SpawnHitParticles(Vector3 position)
    {
        // Instantiate the hit particles prefab at the collision point
        if (hitParticlesPrefab != null)
        {
            Instantiate(hitParticlesPrefab, position, Quaternion.identity);
        }
    }

    void DestroyFireball()
    {
        Destroy(gameObject);
    }
}
