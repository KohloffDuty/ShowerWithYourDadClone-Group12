using UnityEngine;
using UnityEngine.SceneManagement;

public class VanillaCollider : MonoBehaviour
{
	public int points = 10;
	public GameObject VanillaDad; // Reference to the correct dad

	private bool roundEnded = false;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (roundEnded) return;

		Debug.Log($"{gameObject.name} (son) collided with {collision.gameObject.name}");

		// Handle obstacles first
		if (collision.CompareTag("Obstacle"))
		{
			roundEnded = true;

			Debug.Log("Hit an obstacle!");
			return;
		}

		// Check if this is the correct dad
		if (collision.gameObject == VanillaDad)
		{
			HandleSuccess("Found correct dad!");
		}
		// Check if this is a wrong dad (another vanilla)
		else if (collision.CompareTag("EnemyW") || collision.CompareTag("Vanilla"))
		{
			HandleSuccess("Found correct dad!");
		}
		// Anything else
		else
		{
			HandleWrongCollision("Unexpected collision!");
		}
	}

	private void HandleSuccess(string message)
	{
		roundEnded = true;

		if (UIPanel.Instance1 != null)
		{
			UIPanel.Instance1.AddScore(points);
		}

		if (WaveSpawner.Instance != null)
		{
			WaveSpawner.Instance.PlaySound(WaveSpawner.Instance.Coin);
		}

		Debug.Log(message);
		SafeEndRound();
	}

	private void HandleWrongCollision(string message)
	{
		roundEnded = true;
		if (WaveSpawner.Instance != null)
		{
			WaveSpawner.Instance.PlaySound(WaveSpawner.Instance.WrongHit);
		}

		Debug.Log(message);
		SafeEndRound();
	}

	private void SafeEndRound()
	{
		if (Player.instance2 != null)
		{
			Player.instance2.EndRoundAndContinue();
		}
		else
		{
			Debug.LogWarning("Player instance is null, cannot end round");
			// Use the non-deprecated method
			Player player = FindAnyObjectByType<Player>();
			if (player != null)
			{
				WaveSpawner.Instance.EmergencyEndRoundAndContinue();
			}
		}
	}

	public void ResetRoundFlag()
	{
		roundEnded = false;
	}
}