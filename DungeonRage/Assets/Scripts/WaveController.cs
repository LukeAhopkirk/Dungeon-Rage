using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public List<RoomController.EnemyType> enemyTypes;
        public float timeBeforeNextWave;
    }

    public RoomController roomController; // Reference to the RoomController
    public List<Wave> waves;
    private int currentWaveIndex = 0;

    void Start()
    {
        StartCoroutine(SpawnWavesCoroutine());
    }

    private IEnumerator SpawnWavesCoroutine()
    {
        while (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];

            yield return new WaitForSeconds(currentWave.timeBeforeNextWave);

            foreach (var enemyType in currentWave.enemyTypes)
            {
                StartCoroutine(SpawnEnemiesCoroutine(enemyType));
            }

            currentWaveIndex++;
        }
    }

    private IEnumerator SpawnEnemiesCoroutine(RoomController.EnemyType enemyType)
    {
        Bounds bounds = roomController.roomCollider.bounds;

        while (enemyType.EnemyCount < enemyType.maxEnemies)
        {
            // Spawn an enemy
            Vector2 spawnPosition = roomController.GetRandomSpawnPosition(bounds);
            Instantiate(enemyType.prefab, spawnPosition, Quaternion.identity);
            enemyType.IncrementEnemyCount(); // Increment the private enemy count

            // Random interval before spawning the next enemy
            float spawnInterval = Random.Range(roomController.minSpawnInterval, roomController.maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
