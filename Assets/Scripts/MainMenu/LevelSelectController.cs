using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;

public class LevelSelectController : MonoBehaviour
{
	public GameObject unfinishedLevelPrefab;
	public GameObject finishedLevelPrefab;

	public bool unlockAllLevels = false;

	public Button playLevelButton;

	IList<string> levels;
	int currentLevel;
	int maxLevelComplete = -1;

	Coordinate[] levelCoordinates = new Coordinate[]
	{
		new Coordinate(3, 0),
		new Coordinate(3, 1),
		new Coordinate(3, 2),
		new Coordinate(3, 3),
		new Coordinate(2, 3),
		new Coordinate(1, 3),
		new Coordinate(0, 3),
		new Coordinate(0, 2),
		new Coordinate(0, 1),
		new Coordinate(0, 0),
		new Coordinate(1, 0),
		new Coordinate(2, 0)
	};

	// Use this for initialization
	void Start ()
	{
		levels = GameManager.game.Levels;

		// Only go up to the highest completed level;
		for (int i = 0; i < levels.Count; i++)
		{
			if (GameManager.game.GetCompletion(levels[i]))
			{
				maxLevelComplete = i;
			}
		}
		for (int i = 0; i < AvailableLevelCount(); i++)
		{
			var level = levels[i];
			var prefab = GameManager.game.GetCompletion(level) ? finishedLevelPrefab : unfinishedLevelPrefab;
			var newObject = gameObject.AddChild(prefab, levelCoordinates[i]);
			var creatureName = level[0].ToString().ToUpper() + level.Substring(1);
			var creatureType = new CreatureType(creatureName);
			newObject.GetComponentInChildren<SpriteRenderer>().sprite = ResourcesPathfinder.GoalSprite(creatureType);
		}

		LevelManager.Creatures.CreatureStepped += UpdateLevel;
	}

	void UpdateLevel(Creature creature)
	{
		var index = Array.IndexOf(levelCoordinates, creature.Position, 0, AvailableLevelCount());
		if (index >= 0)
		{
			playLevelButton.gameObject.SetActive(true);
			currentLevel = index;
			playLevelButton.GetComponentInChildren<Text>().text = "Play Level " + (currentLevel + 1);
		}
		else
		{
			playLevelButton.gameObject.SetActive(false);
		}
	}

	private int AvailableLevelCount()
	{
		return unlockAllLevels ? levels.Count : Math.Min(maxLevelComplete + 2, levels.Count);
	}

	public void LoadCurrentLevel()
	{
		GameManager.LoadLevel(levels[currentLevel]);
	}

	// Clear our game progress
	public void ClearProgress()
	{
		foreach (string level in GameManager.game.Levels)
		{
			GameManager.game.SetCompletion(level, false);
		}
		GameManager.game.Save();

		// Reload the level
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
