using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

public class LevelManager : MonoBehaviour
{

	// make game manager accessible throughout the game
	public static LevelManager level;

	public static TerrainController Terrain { get { return level.terrainController; } }
	public static CreatureController Creatures { get { return level.creatureController; } }
	public static ResourceController Resources { get { return level.resourceController; } }
	public static RecipeController Recipes { get { return level.recipeController; } }
	public static GoalController Goals { get { return level.goalController; } }

	/*
	 * Public members that can be set per level.
	 */
	// the goal in the world
//	public Goal goal;

	// the grid of available game blocks
	public TerrainController terrainController;
	// the creatures in the game world
	public CreatureController creatureController;
	// the resources in the game world
	public ResourceController resourceController;
	// the recipes of the game world
	public RecipeController recipeController;
	// the goals of the game world
	public GoalController goalController;
	
	// how width of the space between individual cells
	public float cellSize = 2;

	// how fast the game moves
	public float stepInterval = 0.5f;

	public float Steps { get; private set; }

	public event Action Step;

	/*
	 * Internal game state
	 */

	// The game state
	public bool HasWon
	{ 
		get { return hasWon; }
		set
		{
			hasWon = value;
			if (hasWon)
			{
				if (OnWin != null) { OnWin(); }
				// Save that we have won this level
				Debug.Log("Won this level! Saving...");
				GameManager.game.SetCompletion(SceneManager.GetActiveScene().name, true);
				GameManager.game.Save();
			}
		}
	}

	// Event that is called when we are victorious.
	public Action OnWin;

	// the next time to take a step
	private float nextStepTime;

	private bool hasWon;

	void Awake ()
	{
		if (level == null) { level = this; }
		HasWon = false;

		nextStepTime = 0;
		Steps = 0;

		// Auto-wire the controllers if necessary
		if (creatureController == null)
		{
			creatureController = GetComponentInChildren<CreatureController>();
		}
		if (terrainController == null)
		{
			terrainController = GetComponentInChildren<TerrainController>();
		}
		if (resourceController == null)
		{
			resourceController = GetComponentInChildren<ResourceController>();
		}
		if (recipeController == null)
		{
			recipeController = GetComponentInChildren<RecipeController>();
		}
		if (goalController == null)
		{
			goalController = GetComponentInChildren<GoalController>();
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!HasWon)
		{
			if (Time.timeSinceLevelLoad >= nextStepTime)
			{
				nextStepTime += stepInterval;
				Steps++;
				Debug.Log("Current step: " + Steps);
				Step();
			}
		}
	}

}
