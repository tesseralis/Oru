using UnityEngine;
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
		var maxLevelComplete = -1;
		for (int i = 0; i < levels.Count; i++)
		{
			if (GameManager.game.GetCompletion(levels[i]))
			{
				maxLevelComplete = i;
			}
		}
		for (int i = 0; i <= (unlockAllLevels ? levels.Count : maxLevelComplete + 1); i++)
		{
			var level = levels[i];
			var prefab = GameManager.game.GetCompletion(level) ? finishedLevelPrefab : unfinishedLevelPrefab;
			gameObject.AddChild(prefab, levelCoordinates[i]);
		}

		LevelManager.Creatures.CreatureStepped += UpdateLevel;
	}

	public void UpdateLevel(Creature creature)
	{
		if (levelCoordinates.Contains(creature.Position))
		{
			playLevelButton.gameObject.SetActive(true);
			currentLevel = Array.IndexOf(levelCoordinates, creature.Position);
			playLevelButton.GetComponentInChildren<Text>().text = "Play Level " + (currentLevel + 1);
		}
		else
		{
			playLevelButton.gameObject.SetActive(false);
		}
	}

	public void LoadCurrentLevel()
	{
		GameManager.game.LoadLevel(levels[currentLevel]);
	}
}
