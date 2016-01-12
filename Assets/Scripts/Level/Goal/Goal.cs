using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;

public class Goal : MonoBehaviour
{

	// What creature is needed to win the game
	// TODO generalize this so that we can do multiple creatures or terrain or boulders as well
	public CreatureType winningCreatureType;

	void Start()
	{
		LevelManager.Creatures.CreaturesUpdated += CheckGoalCondition;
	}

	private void CheckGoalCondition(IList<Creature> creatureList)
	{
		if (GoalConditionMet(creatureList))
		{
			Debug.Log("You win!");
			LevelManager.Goals.CompleteLevel();
		}
	}

	private bool GoalConditionMet(IList<Creature> creatureList)
	{
		return creatureList.Any(x => x.Position == gameObject.Coordinate()
			&& x.creatureType == winningCreatureType);
	}

}

