using UnityEngine;
using UnityEngine.SceneManagement;
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

	// the winner panel to display when you win
	public GameObject winnerPanel;

	// the level to load when restarting
	public string playAgainLevelToLoad;

	// the goal in the world
	public Goal goal;

	// TODO auto-wire these
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
	private bool hasWon = false;

	// The current time
	private float currentTime;
	// the next time to take a step
	private float nextStepTime;


	void Awake ()
	{
		if (gm == null)
		{
			gm = this.gameObject.GetComponent<GameManager>();
		}
		
		currentTime = 0;
		nextStepTime = 0;
	}
	
	// Use this for initialization
	void Start ()
	{

		if (winnerPanel)
		{
			winnerPanel.SetActive(false);
		}


	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!hasWon)
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
			WinGame();
		}

	}

	private void WinGame()
	{
		hasWon = true;
		if (winnerPanel)
		{
			winnerPanel.SetActive(true);
		}
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(playAgainLevelToLoad);
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
