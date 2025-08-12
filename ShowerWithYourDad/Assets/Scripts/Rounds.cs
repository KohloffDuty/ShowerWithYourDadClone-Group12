using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public GameObject[] EnemiesInWave;
        public int NumberToSpawn;
        public float TimeBeforeThisWave; // Delay before starting this wave
        public float enemyLifetime = 10f; // Time before enemy gets destroyed
        public float roundDuration = 15f; // Time before wave ends
    }

    public Wave[] waves;
    [SerializeField] private Transform[] spawnpoints;

    private Wave currentWave;
    private float nextWaveTime;
    private int currentWaveIndex = 0;
    private bool stopSpawning = false;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Awake()
    {
        currentWave = waves[currentWaveIndex];
        nextWaveTime = Time.time + currentWave.TimeBeforeThisWave;
    }

    private void Update()
    {
        if (stopSpawning)
            return;

        if (Time.time >= nextWaveTime)
        {
            StartCoroutine(HandleWave());
        }
    }

    private IEnumerator HandleWave()
    {
        // Spawn enemies
        SpawnWave();

        // Wait until round duration is over
        yield return new WaitForSeconds(currentWave.roundDuration);

        // End the wave (destroy all remaining enemies)
        EndCurrentWave();

        // Move to the next wave
        IncWave();

        // Start the next wave immediately (optional: remove delay)
        nextWaveTime = Time.time + currentWave.TimeBeforeThisWave;
    }

    private void SpawnWave()
    {
        for (int i = 0; i < currentWave.NumberToSpawn; i++)
        {
            int enemyIndex = Random.Range(0, currentWave.EnemiesInWave.Length);
            int spawnIndex = Random.Range(0, spawnpoints.Length);

            GameObject enemy = Instantiate(
                currentWave.EnemiesInWave[enemyIndex],
                spawnpoints[spawnIndex].position,
                spawnpoints[spawnIndex].rotation
            );

            activeEnemies.Add(enemy);

            // Optional: still destroy after lifetime
            Destroy(enemy, currentWave.enemyLifetime);
        }
    }

    private void EndCurrentWave()
    {
        // Destroy all remaining enemies in the list
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        activeEnemies.Clear();
    }

    private void IncWave()
    {
        if (currentWaveIndex + 1 < waves.Length)
        {
            currentWaveIndex++;
            currentWave = waves[currentWaveIndex];
        }
        else
        {
            stopSpawning = true;
            Debug.Log("All waves completed!");
        }
    }
}
