using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Util;

public class GoalController : MonoBehaviour
{
	public GameObject goalPrefab;
	public bool levelComplete = false;

	public void CompleteLevel()
	{
		if (!levelComplete)
		{
			if (LevelCompleted != null) { LevelCompleted(); }
			// Save that we have won this level
			Debug.Log("Won this level! Saving...");
			GameManager.game.SetCompletion(LevelManager.levelName, true);
			GameManager.game.Save();
		}
		levelComplete = true;
	}

	// Event that is called when we are victorious.
	public event Action LevelCompleted;

	// TODO refactor this to allow multiple goals
	public Goal goal;

	public void SetGoal(Coordinate coordinate, CreatureType type)
	{
		if (goal)
		{
			Destroy(goal.gameObject);
		}

		goal = gameObject.AddChildWithComponent<Goal>(goalPrefab, coordinate);
		goal.winningCreatureType = type;
	}
}
