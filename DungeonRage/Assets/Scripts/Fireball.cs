using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public SkillPointManager skillPointManager;
    public float baseDamage = 35f; // Base damage without multiplier
    public float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
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
            //if its a basic monster
            if(monsterController!= null)
            {
                monsterController.TakeDamage(totalDamage);
            }
            //if its a tank enemy
            else if(tankController != null)
            {
                tankController.TakeDamage(totalDamage);
            }
            //if its a ranged enemy
            else if (rangeController != null)
            {
                rangeController.TakeDamage(totalDamage);
            }

                Debug.Log($"Total damage {totalDamage}");
            // Show debug log for damage dealt
            //Debug.Log($"Fireball dealt {totalDamage} damage to {enemy.gameObject.name}");


            Destroy(gameObject);
        }
    }
}
