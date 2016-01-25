using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the UI (Canvas) components of the game.
/// </summary>
public class UIManager : MonoBehaviour
{
	// make game manager accessible throughout the game
	public static UIManager ui;

	public string menuScene = "Menu";

	void Awake()
	{
		if (ui == null) { ui = this; }
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene(menuScene);
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void NextLevel()
	{
		var levels = GameManager.game.Levels;
		GameManager.game.LoadLevel(levels[levels.IndexOf(LevelManager.levelName) + 1]);
	}

	public void TogglePause()
	{
		UXManager.Time.TogglePause();
	}

}


