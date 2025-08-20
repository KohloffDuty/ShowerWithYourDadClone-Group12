using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class StartPanel : MonoBehaviour
{
	//public Button start;
	private bool madeTop10 = false;
	public LeaderBoard leaderboard;
	public TMPro.TextMeshProUGUI congratsText;
	public TMPro.TextMeshProUGUI enterInitialsText;
	public TMPro.TextMeshProUGUI scoreText;
	//public TMPro.TextMeshProUGUI gameStateText;
	public TMP_InputField initialsInput;
	//public static string playerInitials;
	// Start is called once before the first execution of Update after the MonoBehaviour is created

	public void Start()
	{		
		string sceneName = SceneManager.GetActiveScene().name;

		switch (sceneName)
		{
			case "Start":
				break;

			case "KulaniDuplicate":
				break;

			case "End":
				scoreText.text = "Your final score was: " + UIPanel.Instance1.score.ToString();

				//gameStateText.text = GameInfo.PlayerWon ? "YOU WON" : "YOU LOST";
				
				if (leaderboard.MadeIt(UIPanel.Instance1.score))
				{
					madeTop10 = true;
					initialsInput.gameObject.SetActive(true);
					congratsText.gameObject.SetActive(true);
					enterInitialsText.gameObject.SetActive(true);

					initialsInput.contentType = TMP_InputField.ContentType.Alphanumeric;
					initialsInput.characterLimit = 3;
					initialsInput.onValidateInput += (string text, int charIndex, char addedChar) =>
					{
						return char.ToUpperInvariant(addedChar);
					};

					// Set focus to the text input field so that the user's initials can be captured
					initialsInput.Select();
					initialsInput.ActivateInputField();
				}
				break;
		}
	}

	public void Update()
	{
		if (SceneManager.GetActiveScene().name == "End")
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				if (madeTop10) { leaderboard.Save(initialsInput.text, UIPanel.Instance1.score); }
				SceneManager.LoadScene("Start");
			}
		}
	}

	public void LoadScene()
	{
		SceneManager.LoadScene("KulaniDuplicate");
	}

	public void Restart()
	{
		SceneManager.LoadScene("KulaniDuplicate");
	}

	public void Quit()
	{
		Application.Quit();
	}
}
