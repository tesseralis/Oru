using UnityEngine;
using System.Collections;

public class WinnerPanel : MonoBehaviour
{
	public GameObject nextLevelButton;

	void Start ()
	{
		// Start creating the recipe
		LevelManager.Goals.LevelCompleted += DisplayWinInfo;
		gameObject.SetActive(false);
	}

	void DisplayWinInfo()
	{
		gameObject.SetActive(true);

		// Don't show the "next level" button if this is the last level
		var levels = GameManager.game.Levels;
		var isLastLevel = levels.IndexOf(LevelManager.levelName) == levels.Count - 1;
		nextLevelButton.SetActive(!isLastLevel);
	}
}
