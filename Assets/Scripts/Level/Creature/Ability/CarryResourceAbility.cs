using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

public class CarryResourceAbility : MonoBehaviour, IAbility
{
	public int capacity;

	ResourceCollection carrying;

	public ResourceCollection Carrying
	{
		get { return carrying; }
		set { carrying = value; }
	}

	public class Definition : IAbilityDefinition
	{
		public int Capacity { get; set; }
		public string Description()
		{
			return "Pick up " + Capacity + " resources";
		}

		public IAbility AddToCreature(Creature creature)
		{
			var ability = creature.gameObject.AddComponent<CarryResourceAbility>();
			// TODO figure out if there is a way to structure this without leaving these variables public
			ability.capacity = Capacity;
			ability.carrying = ResourceCollection.Empty();
			return ability;
		}
	}

	public string Description()
	{
		if (Carrying.IsEmpty())
		{
			return "Pick up " + capacity;
		}
		else
		{
			return "Put down " + capacity;
		}
	}

	public void Use(Coordinate target)
	{
		var resources = LevelManager.Resources;

		// If we're not carrying anything, pick up things
		if (Carrying.IsEmpty())
		{
			ResourceCollection carrying;
			resources[target] = resources[target].Take(capacity, out carrying);
			Carrying = carrying;
		}
		else
		{
			// Otherwise, put down what we're carrying right now
			// Put down what we're carrying on the coordinate
			if (LevelManager.Terrain.Contains(target) && LevelManager.Terrain[target] != TerrainType.Tree)
			{
				resources[target] += Carrying;
				Carrying = ResourceCollection.Empty();
			}

		}
	}
}
