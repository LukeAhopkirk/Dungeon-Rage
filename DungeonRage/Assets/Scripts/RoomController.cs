using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Collider2D roomCollider;
    public int maxEnemies = 50;
    private int enemyCount = 0;

    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpawnEnemiesCoroutine());
        }
    }


    private IEnumerator SpawnEnemiesCoroutine()
    {
        Bounds bounds = roomCollider.bounds;

        while (enemyCount < maxEnemies)
        {
            // Spawn an enemy
            Vector2 spawnPosition = GetRandomSpawnPosition(bounds);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemyCount++;

            // Random interval before spawning the next enemy
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector2 GetRandomSpawnPosition(Bounds bounds)
    {
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        Vector2 spawnPosition = new Vector2(randomX, randomY);
        return spawnPosition;
    }
}


