using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamageFireball : MonoBehaviour
{
    public SkillPointManager skillPointManager;
    public float baseDamage = 20f; // Base damage without multiplier
    public float areaDamageRadius = 3f; // Radius for area damage
    public float damage;

    [SerializeField] private ParticleSystem particleSystem = default;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            SpawnHitParticles(transform.position);
            DealAreaDamage();
        }

        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Tank") || collision.gameObject.CompareTag("Range"))
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
                if (collider.CompareTag("Enemy") || collider.CompareTag("Tank") || collider.CompareTag("Range"))
                {

                    // Check if the enemy has the MonsterController component
                    MonsterController monsterController = collider.GetComponent<MonsterController>();
                    // Check if the enemy has the TankController component
                    TankController tankController = collider.GetComponent<TankController>();
                    // Check if the enemy has the RangeController component
                    RangeController rangeController = collider.GetComponent<RangeController>();

                    RageSystem rageSystem = FindObjectOfType<RageSystem>();
                    if (rageSystem != null)
                    {
                        rageSystem.DealDamage(totalDamage);
                    }

                    if (monsterController!= null)
                    {
                        monsterController.TakeDamage(totalDamage);
                    }

                    else if(tankController != null)
                    {
                        tankController.TakeDamage(totalDamage);
                    }
                    else if(rangeController != null)
                    {
                        rangeController.TakeDamage(totalDamage);
                    }
                }
            }
        }
        SpawnHitParticles(transform.position);
        Destroy(gameObject);
    }

    void SpawnHitParticles(Vector3 position)
    {
        // Instantiate the hit particles prefab at the collision point
        Debug.Log("Spawning hit particles");

        if (particleSystem != null)
        {
            Instantiate(particleSystem, position, Quaternion.identity);
        }
    }
}
