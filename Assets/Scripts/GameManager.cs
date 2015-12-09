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
	public GameObject goal;

	// the grid of available game blocks
	public TerrainManager terrain;
	// the creatures in the came world
	public CreatureManager creatures;
	
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


	// The location that needs 
	private Coordinate goalCoordinate;

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

		if (goal)
		{
			goalCoordinate = ToGridCoordinate(goal.transform.position);
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
		creatures.Step();

		if (goalCoordinate != null)
		{
			if (creatures.Creatures.Select(x => x.Position).Contains(goalCoordinate))
			{
				// TODO end game
				Debug.Log("You win!");
				WinGame();
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
