using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinnerPanel : MonoBehaviour {

	public string playAgainLevelToLoad = "LevelPrototype";

	public void RestartGame()
	{
		SceneManager.LoadScene(playAgainLevelToLoad);
	}
}
