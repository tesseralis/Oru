using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Class responsible for controlling the flow of time in the game,
/// including how often the game should update and pausing the game.
/// </summary>
public class TimeController : MonoBehaviour
{

	// How many seconds are in a single "step" in the game world
	public float stepInterval = 0.3f;

	public bool IsPaused { get; private set; }

	// event to call when the game is paused/unpaused
	public event Action<bool> PauseToggled;

	private float nextStepTime = 0f;

	void Awake ()
	{
		// Ensure that the game is running at the right speed
		Time.timeScale = 1f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.timeSinceLevelLoad >= nextStepTime)
		{
			nextStepTime += stepInterval;
			LevelManager.Creatures.StepCreatures();
		}
	}

	// Pause/unpause the game
	public void TogglePause()
	{
		if (IsPaused)
		{
			IsPaused = false;
			Time.timeScale = 1f;
		}
		else
		{
			IsPaused = true;
			Time.timeScale = 0f;
		}
		if (PauseToggled != null) { PauseToggled(IsPaused); }
	}
}
