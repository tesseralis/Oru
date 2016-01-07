using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

public abstract class AbstractCarryAbility : MonoBehaviour, IAbility
{
	private Coordinate? target;

	protected Creature creature;

	// FIXME cancel action when creature goal is set
	void Awake()
	{
		creature = GetComponent<Creature>();
	}

	public void Passive()
	{
		if (target.HasValue && creature.Position.CardinalNeighbors().Contains(target.Value))
		{
			DoPickup(target.Value);
			target = null;
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
			// Otherwise, try to set the location as a goal
			foreach (var neighbor in coordinate.CardinalNeighbors())
			{
				// TODO also account for shortest path
				if (creature.CanReach(neighbor))
				{
					target = coordinate;
					creature.SetGoal(neighbor);
				}
			}
		}
	}

	protected abstract void DoPickup(Coordinate target);

	public abstract string Description();

}

