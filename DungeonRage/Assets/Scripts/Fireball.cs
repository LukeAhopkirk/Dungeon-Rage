using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public SkillPointManager skillPointManager;
    public float baseDamage = 30f; // Base damage without multiplier
    public float damage;

    public void Start()
    {
        skillPointManager = GameObject.FindObjectOfType<SkillPointManager>();
        foreach (var stat in skillPointManager.stats)
        {
            if (stat.statName == "Intelligence")
            {
                stat.OnStatChanged += UpdateDamage;

            }
        }
    }
    void UpdateDamage(float newIntelligence)
    {
        damage = baseDamage + newIntelligence * 5f;
        Debug.Log($"Updated Fireball damage. New Intelligence: {newIntelligence}, New Damage: {damage}");
    }

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
                    float totalDamage = damage * playerMovement.damageMultiplier;

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
