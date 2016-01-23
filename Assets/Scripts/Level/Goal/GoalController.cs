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

	// Add a goal to the list of goals
	public void AddGoal(Coordinate coordinate, CreatureType type)
	{
		if (goal)
		{
			throw new InvalidOperationException("There is already a goal set; we can currently only have one goal per level.");
		}
		var prefab = ResourcesPathfinder.GoalPrefab();
		goal = gameObject.AddChildWithComponent<Goal>(prefab, coordinate);
		goal.winningCreatureType = type;
	}

	void Start()
	{
		if (!goal)
		{
			goal = GetComponentInChildren<Goal>();
		}
	}
}
