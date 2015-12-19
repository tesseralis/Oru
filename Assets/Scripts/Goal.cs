using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using Util;

public class Goal : MonoBehaviour
{

	// What creature is needed to win the game
	// TODO generalize this so this can be "any creature" or terrain (or boulders)
	public CreatureType winningCreatureType;

	void Start()
	{
		GameManager.gm.Step += StepGoal;
	}

	private void StepGoal()
	{
		if (GameManager.Creatures.CreatureList.Where(x => x.creatureType == winningCreatureType)
			.Select(x => x.Position).Contains(this.Coordinate()))
		{
			Debug.Log("You win!");
			GameManager.gm.HasWon = true;
		}
	}

}

