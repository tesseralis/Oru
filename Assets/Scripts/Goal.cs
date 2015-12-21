using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;

public class Goal : MonoBehaviour
{

	// What creature is needed to win the game
	// TODO generalize this so this can be "any creature" or terrain (or boulders)
	public CreatureType winningCreatureType;

	void Start()
	{
		GameManager.Creatures.CreaturesUpdated += CheckGoalCondition;
	}

	private void CheckGoalCondition(IList<Creature> creatureList)
	{
		if (creatureList.Where(x => x.creatureType == winningCreatureType)
			.Select(x => x.Position).Contains(gameObject.Coordinate()))
		{
			Debug.Log("You win!");
			GameManager.gm.HasWon = true;
		}
	}

}

