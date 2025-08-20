using TMPro;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
	public float startTime = 30f; // Time in seconds
	private float currentTime;

	//public TMP_Text timerText; // Assign in Inspector
	public TMP_Text scoreText;
	public int score = 0;
	public static UIPanel Instance1;
	//public GameObject endPanel;
	private void Awake()
	{
		if (Instance1 == null)
		{
			Instance1 = this;
		}
		Time.timeScale = 1.5f;
	}
	void Start()
	{
		currentTime = startTime;
		score = 0;
		ScoreUpdate();
	}

	void Update()
	{
		// Count down using actual time
		currentTime -= Time.deltaTime;

		// Prevent going below zero
		if (currentTime < 0)
		{
			currentTime = 0;
		}

	}

	// Updates the score
	public void AddScore(int amount)
	{
		score += amount;
		ScoreUpdate();
	}

	void ScoreUpdate()
	{
		scoreText.text = "Score: " + score;
		UpdateScoreUI();
	}

	private void UpdateScoreUI()
	{
		if (scoreText != null)
			scoreText.text = score.ToString();
	}

}
