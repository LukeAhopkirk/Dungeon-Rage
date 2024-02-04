using System.Collections;
using UnityEngine;

public class Outburst : MonoBehaviour
{
    public GameObject fireballPrefab;  // Reference to the fireball prefab
    public GameObject fireballTrackingPrefab;
    public float trackingFireballRadius = 5f;  // Radius for tracking fireballs
    public float force = 10f;  // Force applied to fireballs

    private bool isRunning = false;

    // Start the special attack when called
    public void StartOutburst()
    {
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(SpawnRandomDirectionFireballsCoroutine());
            StartCoroutine(SpawnTrackingFireballsCoroutine());
        }
    }

    // Stop the special attack when called
    public void StopOutburst()
    {
        if (isRunning)
        {
            isRunning = false;
            StopAllCoroutines();
        }
    }

    IEnumerator SpawnRandomDirectionFireballsCoroutine()
    {
        while (isRunning)
        {
            // Spawn 6 random direction fireballs simultaneously
            SpawnRandomDirectionFireballs(6);

            // Wait for 0.3 seconds before the next set
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator SpawnTrackingFireballsCoroutine()
    {
        while (isRunning)
        {
            // Spawn 1 tracking fireball
            SpawnTrackingFireball();

            // Wait for 0.6 seconds before the next tracking fireball
            yield return new WaitForSeconds(0.1f);
        }
    }

    void SpawnRandomDirectionFireballs(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Instantiate the fireball object
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

            // Set a random direction for the fireball
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            fireball.GetComponent<Rigidbody2D>().velocity = randomDirection * force;

            Physics2D.IgnoreCollision(fireball.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    void SpawnTrackingFireball()
    {
        // Find the nearest enemy within the tracking radius
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            GameObject fireball = Instantiate(fireballTrackingPrefab, transform.position, Quaternion.identity);

            // Set the fireball to track the nearest enemy
            TrackingFireball trackingFireball = fireball.GetComponent<TrackingFireball>();
            trackingFireball.SetTarget(nearestEnemy.transform); // Pass the Transform

            Physics2D.IgnoreCollision(fireball.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    GameObject FindNearestEnemy()
    {
        // Find all objects with the "Enemy" tag within the tracking radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, trackingFireballRadius);

        if (colliders.Length > 0)
        {
            // Select the nearest enemy from the list
            GameObject nearestEnemy = null;
            float minDistance = float.MaxValue;

            foreach (Collider2D collider in colliders)
            {
                // Check if the collider has the "Enemy" tag
                if (collider.CompareTag("Enemy"))
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = collider.gameObject;
                    }
                }
            }

            return nearestEnemy;
        }

        return null;
    }
}