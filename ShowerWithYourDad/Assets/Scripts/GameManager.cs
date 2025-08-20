using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	public static GameEvents instance;

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(gameObject);
	}

	// Events
	public event Action<int> OnScore;           // Send points
	public event Action OnRoundSuccess;         // Player found dad
	public event Action OnRoundFail;            // Wrong or obstacle hit

	// Raise methods
	public void RaiseScore(int points) => OnScore?.Invoke(points);
	public void RaiseRoundSuccess() => OnRoundSuccess?.Invoke();
	public void RaiseRoundFail() => OnRoundFail?.Invoke();
}
