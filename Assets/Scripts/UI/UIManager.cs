using UnityEngine;

/// <summary>
/// Manages the UI (Canvas) components of the game.
/// </summary>
public class UIManager : MonoBehaviour
{
	// make game manager accessible throughout the game
	public static UIManager ui;

	void Awake()
	{
		if (ui == null) { ui = this; }
	}

	public void ReturnToMenu()
	{
		GameManager.LoadMainMenu();
	}

	public void RestartLevel()
	{
		GameManager.LoadLevel(GameManager.game.currentLevel);
	}

	public void NextLevel()
	{
		var levels = GameManager.game.Levels;
		var nextLevel = levels[levels.IndexOf(LevelManager.level.levelName) + 1];
		Debug.Log("Loading the next level: " + nextLevel);
		GameManager.LoadLevel(nextLevel);
	}

	public void TogglePause()
	{
		UXManager.Time.TogglePause();
	}

}


