using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinnerPanel : MonoBehaviour {

	public string menuScene = "Menu";

	public void ReturnToMenu()
	{
		SceneManager.LoadScene(menuScene);
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
