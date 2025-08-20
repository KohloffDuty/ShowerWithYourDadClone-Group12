using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderBoard : MonoBehaviour
{
	//TextAsset FilePath = Resources.Load<TextAsset>("ScoreBoard");
	//private static string FilePath => Path.GetFullPath(Path.Combine(Application.dataPath, "Scripts", "ScoreBoard.txt"));
	private string FilePath => Path.Combine(Application.streamingAssetsPath, "ScoreBoard.txt");
	public List<LeaderboardEntry> leaderboardEntries = new();
	public TMPro.TextMeshProUGUI entriesText;

	private void Start()
	{
		if (SceneManager.GetActiveScene().name == "Start") { Load(); }
	}

	public void Save(string newInitials, int newScore)
	{
		int index = -1;

		if (leaderboardEntries.Count == 0) { Load(false); }

		// Update the ranking list with the new score in the correct position
		for (int i = 0; i < leaderboardEntries.Count; i++)
		{
			if (int.TryParse(leaderboardEntries[i].score, out int scoreValue))
			{
				if (newScore > scoreValue)
				{
					index = i;
					break;
				}
			}
			else
			{
				index = i;
				break;
			}
		}

		if (index > -1)
		{
			// Add the new score to the leaderboard list - the rank will be updated once the list is updated
			leaderboardEntries.Insert(index, new LeaderboardEntry() { initials = newInitials, score = newScore.ToString().PadLeft(4, '0') });
			leaderboardEntries.RemoveAt(10);

			// Update the rankings of all the leaderboard list items
			for (int i = index; i < leaderboardEntries.Count; i++)
			{
				leaderboardEntries[i].rank = (i + 1).ToString().PadLeft(2, '0');
			}

			// Add the column headings to the list so that they can be written to the text file
			leaderboardEntries.Insert(0, new LeaderboardEntry() { rank = "Rank", initials = "Name", score = "Score" });

			// Convert each entry to a CSV line: "01,ABC,12345"
			var lines = leaderboardEntries
									.Select(e => $"{e.rank},{e.initials},{e.score}")
									.ToArray();

			// Write (or overwrite) all lines at once
			File.WriteAllLines(FilePath, lines);
		}
	}

	public void Load(bool updateUI = true)
	{
		leaderboardEntries = new List<LeaderboardEntry>();

		if (!File.Exists(FilePath)) { return; }

		if (entriesText != null && updateUI) { entriesText.text = string.Empty; }

		foreach (var line in File.ReadAllLines(FilePath))
		{
			var parts = line.Split(',');
			if (parts.Length > 0 && parts[0] != "Rank")
			{
				if (parts.Length == 3)
				{
					// Add the line to the leaderboard textbox
					if (entriesText != null && updateUI) { entriesText.text += (parts[0].Trim() + "\t\t\t" + parts[1].Trim() + "\t\t\t" + parts[2].Trim() + "\r\n"); }

					leaderboardEntries.Add(new LeaderboardEntry
					{
						rank = parts[0],
						initials = parts[1],
						score = parts[2]
					});
				}
			}
		}
	}

	public bool MadeIt(int newScore)
	{
		if (leaderboardEntries.Count == 0) { Load(false); }
		return int.TryParse(leaderboardEntries[leaderboardEntries.Count - 1].score, out int lowestScore) ? (newScore > lowestScore) : true;
	}
}

