using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIPanel : MonoBehaviour
{
	public float startTime = 30f; // Time in seconds
	private float currentTime;

	//public TMP_Text timerText; // Assign in Inspector
	public TMP_Text scoreText;
	public float score = 0;

	//public GameObject endPanel;

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

		// Calculate minutes, seconds, and milliseconds
		//int minutes = Mathf.FloorToInt(currentTime / 60f);
		//int seconds = Mathf.FloorToInt(currentTime % 60f);
		//int milliseconds = Mathf.FloorToInt((currentTime * 1000f) % 1000f);

		//// Format: MM:SS:MS (e.g. 00:29:456)
		//timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

		//if(currentTime == 0)
		//{
		//	endPanel.SetActive(true);
		//}
	}

	// Updates the score
	public void AddScore(float amount)
	{
		score += amount;
		ScoreUpdate();
	}

	void ScoreUpdate()
	{
		scoreText.text = "Score: " + score;
	}
}
