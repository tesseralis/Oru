using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinnerPanel : MonoBehaviour {

	public string playAgainLevelToLoad = "LevelPrototype";

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(playAgainLevelToLoad);
	}
}
