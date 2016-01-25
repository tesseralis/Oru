using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

public class CarryResourceAbility : AbstractCarryAbility, IAbility
{
	public int capacity;

	public ResourceCollection carrying;

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

	public override string Description()
	{
		if (Carrying.IsEmpty())
		{
			return "Pick up " + capacity + " resources";
		}
		else
		{
			return "Drop off resources";
		}
	}

	protected override void DoPickup(Coordinate target)
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

	public override bool CanUse(Coordinate coordinate)
	{
		if (Carrying.IsEmpty())
		{
			return !LevelManager.Resources[coordinate].IsEmpty();
		}
		else
		{
			// Can't put down resources on trees
			return LevelManager.Terrain[coordinate] != TerrainType.Tree;
		}
	}
}
