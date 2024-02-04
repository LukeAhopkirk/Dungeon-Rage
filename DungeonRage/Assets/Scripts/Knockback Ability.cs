using UnityEngine;

public class KnockbackAbility : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackRadius = 3f;

    // Function to be called when using the knockback ability
    public void UseKnockbackAbility()
    {
        // Find all objects with an Enemy tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Check if the enemy has the MonsterController component
            MonsterController monsterController = enemy.GetComponent<MonsterController>();

            if (monsterController != null)
            {
                // Calculate the distance between the player and the enemy
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

                // Check if the enemy is within the knockback radius
                if (distanceToEnemy <= knockbackRadius)
                {
                    // Apply knockback to the enemy
                    Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                    monsterController.Knockback(knockbackDirection, knockbackForce);
                }
            }
        }
    }
}
