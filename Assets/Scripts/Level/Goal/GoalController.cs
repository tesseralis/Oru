using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GoalController : MonoBehaviour
{
	private bool hasWon = false;

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
	public event Action OnWin;

	// FIXME refactor this to allow multiple goals
	public Goal goal;
}
