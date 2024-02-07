using System;
using System.Collections;
using UnityEngine;

public class KnockbackAbility : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackRadius = 3f;

    public GameObject prefab;

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
                    // Apply knockback to the enemy with a uniform force
                    Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                    monsterController.Knockback(knockbackDirection, knockbackForce);
                }
            }
        }
    }

    public void explosionAnim()
    {
        Vector2 pos = transform.position;

        Debug.Log(pos.y);
        pos.y -= 0.2f;
        Debug.Log(pos.y);
        GameObject spell = Instantiate(prefab, pos, Quaternion.identity);

        Animator animator = spell.GetComponent<Animator>();

        //Destroy(spell, animator.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(WaitAndDestroy(animator.GetCurrentAnimatorStateInfo(0).length, spell, animator));
    }

    IEnumerator WaitAndDestroy(float duration, GameObject spellObject, Animator animat)
    {
        // Wait for the duration of the animation
        yield return new WaitForSeconds(duration);

        animat.enabled = false;
        // Wait for additional time before destroying (adjust the duration as needed)
        yield return new WaitForSeconds(2.0f);

        // Destroy the spell GameObject after waiting
        Destroy(spellObject);
    }
}
