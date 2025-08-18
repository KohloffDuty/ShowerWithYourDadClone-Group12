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
        public GameObject[] SonsInWave;
        public int NumberToSpawn;
		public int NumberSonsToSpawn;
		public float TimeBeforeThisWave; // Delay before starting this wave
		public float roundDuration = 15f; // Time before wave ends
	}

	public Wave[] waves;
	[SerializeField] private Transform[] spawnpoints;
    [SerializeField] private Transform[] SonSpawnpoints;

    private int currentWaveIndex = 0;
	public Image timerFillImage; // Assign your UI Image here in the Inspector
	public float totalTime = 60f; // Total time in seconds
	private float currentTime;
	private bool isRoundActive = false;
	private string RoundNumber;
	public Text RoundNumberText;
	public static WaveSpawner Instance;

	private Coroutine waveCoroutine;
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		Time.timeScale = 1.0f;
	}
	private void Start()
	{
		waveCoroutine = StartCoroutine(StartWaves());
		ResetRound();
	}

	void Update()
	{
		if (isRoundActive && currentTime > 0)
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
        GameObject[] existingSons = GameObject.FindGameObjectsWithTag("chocolate");
        foreach (GameObject player in existingSons)
        {
            Destroy(player);
        }
    }

	public void ResetRound()
	{
		currentTime = totalTime;
		timerFillImage.fillAmount = 1f;
	}

    // Utility function to shuffle any list
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            (list[i], list[randIndex]) = (list[randIndex], list[i]);
        }
    }

    private void SpawnWave(Wave wave)
    {
        // --- ENEMY SPAWNING ---
        List<Transform> availableSpawns = new List<Transform>(spawnpoints);

        if (wave.NumberToSpawn > availableSpawns.Count)
        {
            Debug.LogWarning("Not enough unique spawn points for all enemies in this wave.");
        }

        int totalSpawned = 0;

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

                if (totalSpawned >= wave.NumberToSpawn) break;
            }
        }

        // --- SON SPAWNING ---
        if (wave.SonsInWave != null && wave.SonsInWave.Length > 0)
        {
            List<Transform> availableSonSpawns = new List<Transform>(SonSpawnpoints);

            int sonsToSpawn = Mathf.Min(wave.NumberSonsToSpawn, wave.SonsInWave.Length);

            List<GameObject> chosenSons = new List<GameObject>(wave.SonsInWave);

            // Only shuffle if more than one son
            if (chosenSons.Count > 1)
            {
                ShuffleList(chosenSons);
            }

            for (int h = 0; h < sonsToSpawn; h++)
            {
                if (availableSonSpawns.Count == 0)
                {
                    availableSonSpawns = new List<Transform>(SonSpawnpoints); // refill if we run out
                }

                int spawnIndex = Random.Range(0, availableSonSpawns.Count);

				Instantiate(
                    chosenSons[h],
                    availableSonSpawns[spawnIndex].position,
                    availableSonSpawns[spawnIndex].rotation
                );

                availableSonSpawns.RemoveAt(spawnIndex);
            }
        }
    }

    private IEnumerator ShowRoundNumberUI(string text)
	{
		RoundNumberText.text = $"{text}";
		RoundNumberText.gameObject.SetActive(true);

		// Show for 2 seconds
		yield return new WaitForSeconds(5f);

		RoundNumberText.gameObject.SetActive(false);
	}

	public void OnDadClicked()
	{
		if (waveCoroutine != null)
		{
			StopCoroutine(waveCoroutine);
		}

		DestroyPreviousEnemies();
		currentWaveIndex++;

		if (currentWaveIndex < waves.Length)
		{
			waveCoroutine = StartCoroutine(StartWaves());
		}
		else
		{
			Debug.Log("All waves completed!");
		}
	}

}
