using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

public class GameManager : MonoBehaviour
{

	// make game manager accessible throughout the game
	public static GameManager gm;

	public static TerrainController Terrain { get { return gm.terrainController; } }
	public static CreatureController Creatures { get { return gm.creatureController; } }
	public static ResourceController Resources { get { return gm.resourceController; } }
	public static RecipeController Recipes { get { return gm.recipeController; } }

	/*
	 * Public members that can be set per level.
	 */
	// the goal in the world
	public Goal goal;

	// the grid of available game blocks
	public TerrainController terrainController;
	// the creatures in the game world
	public CreatureController creatureController;
	// the resources in the game world
	public ResourceController resourceController;
	// the recipes of the game world
	public RecipeController recipeController;

	
	// how width of the space between individual cells
	public float cellSize = 2;

	// how fast the game moves
	public float stepInterval = 0.5f;

	/*
	 * Internal game state
	 */

	// The game state
	public bool HasWon
	{ 
		get { return hasWon; }
		private set
		{
			hasWon = value;
			if (OnWin != null) { OnWin(); }
		}
	}

	// Event that is called when we are victorious.
	public Action OnWin;

	// The current time
	private float currentTime;
	// the next time to take a step
	private float nextStepTime;

	private bool hasWon;

	void Awake ()
	{
		if (gm == null)
		{
			gm = this.gameObject.GetComponent<GameManager>();
		}
		HasWon = false;
		
		currentTime = 0;
		nextStepTime = 0;
	}
	
	// Initialize the controllers if necessary
	void Start ()
	{
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
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!HasWon)
		{
			currentTime += Time.deltaTime;
			if (currentTime >= nextStepTime)
			{
				nextStepTime += stepInterval;
				Step ();
			}
		}
	}

	// Move one discrete game step
	void Step ()
	{
		Debug.Log ("Next step: " + nextStepTime);
		creatureController.Step();
		recipeController.UpdateAvailableRecipes(creatureController.Creatures.Select(x => x.Position).ToList());

		if (creatureController.Creatures.Where(x => x.creatureType == goal.winningCreatureType)
			.Select(x => x.Position).Contains(goal.Coordinate()))
		{
			Debug.Log("You win!");
			HasWon = true;
		}
	}

	public Coordinate ToGridCoordinate(Vector3 position)
	{
		return new Coordinate(Mathf.RoundToInt(position.x / cellSize), Mathf.RoundToInt(position.z / cellSize));
	}

	public void SetPosition(GameObject gameObject, Coordinate coordinate)
	{
		gameObject.transform.position = new Vector3(coordinate.x * cellSize, gameObject.transform.position.y, coordinate.z * cellSize);
	}
}
