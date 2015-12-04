using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Grid = System.Collections.Generic.IDictionary<Coordinate, Cell.cellTypes>;

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

	// the marker UI for the current game creature
	public GameObject creatureMarker;
	// the goal in the world
	public GameObject goal;

	// the grid of available game tiles
	public GameObject cells;
	// the creatures in the came world
	public GameObject creatures;
	
	// how width of the space between individual cells
	public float cellSize = 2;

	// how fast the game moves
	public float stepInterval = 1.0f;

	/*
	 * Internal game state
	 */

	// The game state
	private bool hasWon = false;

	// the current time
	private float currentTime;
	// the next time to take a step
	private float nextStepTime;

	// the currently selected creature
	private GameObject currentCreature;

	// a map of available cell tiles
	private Grid grid;

	// a map of positions of creatures on the board
	private IDictionary<GameObject, Coordinate> creatureCoordinates;

	// a map of goals for the creatures on the board
	private IDictionary<GameObject, Coordinate> creatureDestinations;

	// The location that needs 
	private Coordinate goalCoordinate;
	
	// Use this for initialization
	void Start ()
	{

		if (gm == null)
		{
			gm = this.gameObject.GetComponent<GameManager>();
		}

		currentTime = 0;
		nextStepTime = 0;
		if (creatureMarker)
		{
			creatureMarker.SetActive (false);
		}

		if (cells)
		{
			grid = InitGrid (cells);
		}

		creatureDestinations = new Dictionary<GameObject, Coordinate> ();

		if (creatures)
		{
			creatureCoordinates = InitCreatures(creatures);
		}

		if (goal)
		{
			goalCoordinate = CalculateGridCoordinate(goal.transform.position);
		}

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
		foreach (GameObject child in creatureDestinations.Keys)
		{
			Debug.Log ("Seeing if we need to move " + child);
			if (!creatureCoordinates[child].Equals(creatureDestinations[child]))
			{
				Debug.Log("Moving " + child + " to " + creatureDestinations[child]);
				DoNextStep(child, creatureDestinations[child]);
			}
		}

		// End the game if there's a creature at the goal
		// TODO move this out to a new function and abstract it out
		if (goalCoordinate != null)
		{
			if (creatureCoordinates.Values.Contains(goalCoordinate))
			{
				// TODO end game
				Debug.Log("You win!");
				WinGame();
			}
		}
	}

	// Initialize what cell are available in the grid to move to
	private Grid InitGrid(GameObject cell)
	{
		Grid grid = new Dictionary<Coordinate, Cell.cellTypes>();
		foreach (Transform child in cell.transform)
		{
			Coordinate childCoord = CalculateGridCoordinate(child.transform.position);
			// TODO Store something more useful here (like cell type)
			grid.Add(childCoord, child.GetComponent<Cell>().cellType);
		}
		return grid;
	}

	// Initialize the positions of the creatures
	private IDictionary<GameObject, Coordinate> InitCreatures(GameObject creatures)
	{
		var positions = new Dictionary<GameObject, Coordinate>();
		foreach (Transform child in creatures.transform)
		{
			Coordinate childCoord = CalculateGridCoordinate (child.transform.position);
			positions[child.gameObject] = childCoord;
		}
		return positions;
	}

	private void WinGame()
	{
		hasWon = true;
		if (winnerPanel)
		{
			winnerPanel.SetActive(true);
		}
	}

	// Set the currently selected creature in the game
	public void SetCurrentCreature(GameObject target)
	{
		currentCreature = target;
		// Set the visual marker for the creature
		if (creatureMarker)
		{
			creatureMarker.SetActive(true);
			creatureMarker.transform.SetParent(target.transform, false);
		}
	}

	// Set the goal for the current creature
	public void SetCurrentCreatureDestination(GameObject target)
	{
		if (currentCreature)
		{
			creatureDestinations[currentCreature] = CalculateGridCoordinate(target.transform.position);
		}
	}

	public void RestartGame()
	{
		Application.LoadLevel(playAgainLevelToLoad);
	}

	// TODO: translate the other way so we don't have to keep using float arith
	private Coordinate CalculateGridCoordinate(Vector3 position)
	{
		return new Coordinate((int)(position.x / cellSize), (int)(position.z / cellSize));
	}
	
	// Move the creature to the next step towards its goal
	private void DoNextStep(GameObject creature, Coordinate goal)
	{
		var next = NextCoordinate (CalculateGridCoordinate (creature.transform.position), goal, grid, creature.GetComponent<Creature>().allowedCells);
		creatureCoordinates[creature] = next;
		creature.transform.position = new Vector3(next.x * cellSize, creature.transform.position.y, next.z * cellSize);
	}

	// do a BFS and figure out the right path
	private Coordinate NextCoordinate(Coordinate start, Coordinate end, Grid grid, Cell.cellTypes[] allowedCellTypes)
	{
		// TODO account for disallowed coordinates
		// TODO do A* search
		var distance = new Dictionary<Coordinate, int>();
		var parents = new Dictionary<Coordinate, Coordinate> ();
		var queue = new Queue<Coordinate> ();
		distance [start] = 0;
		queue.Enqueue (start);

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			// TODO factor out to a separate script
			Coordinate[] neighbors = {
				new Coordinate(current.x, current.z + 1),
				new Coordinate(current.x, current.z - 1),
				new Coordinate(current.x + 1, current.z),
				new Coordinate(current.x - 1, current.z)
			};
			foreach (Coordinate neighbor in neighbors)
			{
				// TODO break when we reach the goal
				if (grid.ContainsKey(neighbor) && allowedCellTypes.Contains(grid[neighbor]) && !distance.ContainsKey(neighbor))
				{
					distance[neighbor] = distance[current] + 1;
					parents[neighbor] = current;
					queue.Enqueue(neighbor);
				}
			}

		}
		var next = end;
		// if we can't get to the destination, return the same item
		if (!parents.ContainsKey(next))
		{
			return start;
		}
		while (!parents[next].Equals (start))
		{
			Debug.Log("x="+next.x + " z="+next.z);
			next = parents[next];
		}
		return next;
	}
}


