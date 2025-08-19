using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartPanel : MonoBehaviour
{
	public Button start;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void LoadScene()
	{
		SceneManager.LoadScene("Duplicate");
	}


}
