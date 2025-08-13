using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public GameObject[] EnemiesInWave;
        public int NumberToSpawn;
        public float TimeBeforeThisWave; // Delay before starting this wave
        public float roundDuration = 15f; // Time before wave ends
    }

    public Wave[] waves;
    [SerializeField] private Transform[] spawnpoints;

    private int currentWaveIndex = 0;
    public Image timerFillImage; // Assign your UI Image here in the Inspector
    public float totalTime = 60f; // Total time in seconds
    private float currentTime;
    private bool isRoundActive = false;
    private string RoundNumber;
    public Text RoundNumberText;
    private void Start()
    {
       StartCoroutine(StartWaves());
        ResetRound();
    }

    void Update()
    {
        if ( isRoundActive && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            timerFillImage.fillAmount = currentTime / totalTime;
        }
        else
        {
            currentTime = 0;
        }
    }

    private IEnumerator StartWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];

            // Wait before this wave starts
            yield return new WaitForSeconds(currentWave.TimeBeforeThisWave);

            // Destroy leftover enemies from the previous wave
            DestroyPreviousEnemies();

            // Reset timer for the new round
            ResetRound(); 
            isRoundActive = true;


            StartCoroutine(ShowRoundNumberUI(currentWave.waveName));

            // Spawn all enemies for this wave
            SpawnWave(currentWave);

            // Wait until round duration ends
            yield return new WaitForSeconds(currentWave.roundDuration);

            currentWaveIndex++;
        }
    }

    private void DestroyPreviousEnemies()
    {
        GameObject[] existingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in existingEnemies)
        {
            Destroy(enemy);
        }
        GameObject[] existingObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in existingObstacles)
        {
            Destroy(obstacle);
        }
    }

    public void ResetRound()
    {
        currentTime = totalTime;
        timerFillImage.fillAmount = 1f;
    }

    private void SpawnWave(Wave wave)
    {
        // Copy spawnpoints to a temporary list so we can remove as we use them
        List<Transform> availableSpawns = new List<Transform>(spawnpoints);

        // Make sure there are enough spawn points (optional check)
        if (wave.NumberToSpawn > availableSpawns.Count)
        {
            Debug.LogWarning("Not enough unique spawn points for all enemies in this wave.");
        }

        int totalSpawned = 0;

        // Loop through all enemy types 

        // k is enemy index 
        for (int k = 0; k < wave.EnemiesInWave.Length; k++)
        {
            for (int count = 0; count < wave.NumberToSpawn / wave.EnemiesInWave.Length; count++)
            {
                if (availableSpawns.Count == 0)
                {
                    availableSpawns = new List<Transform>(spawnpoints); // refill if we run out
                }

                int spawnIndex = Random.Range(0, availableSpawns.Count);

                Instantiate(
                    wave.EnemiesInWave[k],
                    availableSpawns[spawnIndex].position,
                    availableSpawns[spawnIndex].rotation
                );

                availableSpawns.RemoveAt(spawnIndex);
                totalSpawned++;

                if (totalSpawned >= wave.NumberToSpawn) return;
            }
        }
    }
    private IEnumerator ShowRoundNumberUI(string text)
    {
        RoundNumberText.text = $"{text}";
        RoundNumberText.gameObject.SetActive(true);

        // Show for 2 seconds
        yield return new WaitForSeconds(2f);

        RoundNumberText.gameObject.SetActive(false);
    }



}
