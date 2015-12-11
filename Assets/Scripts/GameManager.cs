using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

	// make game manager accessible throughout the game
	public static GameManager gm;

	/*
	 * Public members that can be set per level.
	 */

	// the winner panel to display when you win
	public GameObject winnerPanel;

	// the level to load when restarting
	public string playAgainLevelToLoad;

	// the goal in the world
	public Goal goal;

	// the grid of available game blocks
	public TerrainManager terrain;
	// the creatures in the came world
	public CreatureManager creatures;

	// the set of instructions available in the game world
	public GameObject instructions;

	// the element to use to determine where to create creatures
	public GameObject createMarker;
	
	// how width of the space between individual cells
	public float cellSize = 2;

	// how fast the game moves
	public float stepInterval = 1.0f;

	/*
	 * Internal game state
	 */

	// The game state
	private bool hasWon = false;

	// The current time
	private float currentTime;
	// the next time to take a step
	private float nextStepTime;

	// The location of all the instructions in the world
	private IDictionary<Coordinate, Instruction> instructionLocations;
	// The list of available instructions in the game
	private IList<CreatureType> availableInstructions;

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

		instructionLocations = new Dictionary<Coordinate, Instruction>();
		availableInstructions = new List<CreatureType>();
		if (instructions)
		{
			foreach (var instruction in instructions.GetComponentsInChildren<Instruction>())
			{
				instructionLocations[instruction.Coordinate] = instruction;
			}
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
		creatures.Step();

		if (creatures.Creatures.Where(x => x.creatureType == goal.winningCreatureType)
			.Select(x => x.Position).Contains(goal.Coordinate))
		{
			// TODO end game
			Debug.Log("You win!");
			WinGame();
		}

		foreach (var entry in instructionLocations)
		{
			// If there is a creature at our location, remove this instruction
			// from the grid and add it to the list of available instructions
			if (creatures.Creatures.Select(x => x.Position).Contains(entry.Key))
			{
				if (!availableInstructions.Contains(entry.Value.creature))
				{
					availableInstructions.Add(entry.Value.creature);
				}
				Destroy(entry.Value.gameObject);
			}
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
		// TODO round instead of floor for consistency?
		return new Coordinate((int)(position.x / cellSize), (int)(position.z / cellSize));
	}
	
}
