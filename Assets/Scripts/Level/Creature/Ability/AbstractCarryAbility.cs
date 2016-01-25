using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

public abstract class AbstractCarryAbility : MonoBehaviour, IAbility
{
	public bool isActing;

	protected Creature creature;

	// TODO cancel action when creature goal is set
	void Awake()
	{
		creature = GetComponent<Creature>();
	}

	public void Passive()
	{
		if (isActing && creature.NextPosition.CardinalNeighbors().Contains(creature.Goal))
		{
			DoPickup(creature.Goal);
			isActing = false;
			creature.SetGoal(creature.NextPosition);
		}
	}

	public void Use(Coordinate coordinate)
	{
		var direction = coordinate - creature.Position;
		if (Coordinate.cardinals.Contains(direction) || coordinate == creature.Position)
		{
			DoPickup(coordinate);
		}
		else
		{
			creature.SetGoal(coordinate);
			isActing = true;
		}
	}

	public void Cancel()
	{
		isActing = false;
	}

	protected abstract void DoPickup(Coordinate target);

	public abstract string Description();

	public abstract bool CanUse(Coordinate coordinate);

}

