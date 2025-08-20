using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public UIPanel uiPanel;
    public WaveSpawner waveSpawner;

    [Header("Sound Clips")]
    public AudioClip coinSound;
    public AudioClip wrongHitSound;
   // public AudioClip obstacleHitSound;

    // Events for game actions
    public static event Action OnRoundEndRequested;
    public static event Action<int> OnScoreAddRequested;
    public static event Action<AudioClip> OnSoundPlayRequested;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore1(int points)
    {
        // Update UI directly
        if (uiPanel != null)
            uiPanel.AddScore(points);

        // Also notify through event for any other systems
        OnScoreAddRequested?.Invoke(points);
    }

    public void PlaySound(AudioClip clip)
    {
        // Play sound directly
        if (waveSpawner != null && clip != null)
            waveSpawner.PlaySound(clip);

        // Also notify through event
        OnSoundPlayRequested?.Invoke(clip);
    }

    public void PlayCoinSound() => PlaySound(coinSound);
    public void PlayWrongHitSound() => PlaySound(wrongHitSound);
   // public void PlayObstacleHitSound() => PlaySound(obstacleHitSound);

    public void EndRound()
    {
        OnRoundEndRequested?.Invoke();
    }

    public void HandleCollisionSuccess(int points, string message)
    {
        AddScore1(points);
        PlayCoinSound();
        Debug.Log(message);
        EndRound();
    }

    public void HandleCollisionFailure(string message, bool playSound = true)
    {
        if (playSound)
            PlayWrongHitSound();
        Debug.Log(message);
        EndRound();
    }

    public void HandleObstacleCollision(string message)
    {
       // PlayObstacleHitSound();
        Debug.Log(message);
        EndRound();
    }
    public void EndRoundAndContinue()
    {
        // Your existing round end logic
        Debug.Log("Round ended via event");

        // Destroy leftover enemies/sons
        waveSpawner.DestroyPreviousEnemies();

        // Reset round state
        waveSpawner.ResetRound();

        // Start the next round after 2 seconds
        StartCoroutine(StartNextRound());
    }
    public System.Collections.IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(2f); // Delay before next wave
        StartCoroutine(waveSpawner.StartWaves());
    }
    
}