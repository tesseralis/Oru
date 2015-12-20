using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinnerPanel : MonoBehaviour {

	public string menuScene = "Menu";

	// TODO refactor these methods so that they can be used at any point in the level
	public void ReturnToMenu()
	{
		SceneManager.LoadScene(menuScene);
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
