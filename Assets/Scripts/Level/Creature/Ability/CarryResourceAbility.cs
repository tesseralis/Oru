using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

public class CarryResourceAbility : AbstractCarryAbility, IAbility
{
	public int capacity;

	public ResourceCollection carrying;

	private ResourcePile resourcePile;

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
			return "Carry up to " + Capacity + " items";
		}

		public IAbility AddToCreature(Creature creature)
		{
			var ability = creature.gameObject.AddComponent<CarryResourceAbility>();
			// TODO figure out if there is a way to structure this without leaving these variables public
			ability.capacity = Capacity;
			ability.carrying = ResourceCollection.Empty();

			// Add a visual indicator for what the creature is carrying
			// TODO perhaps this should be in some UX class?
			ability.resourcePile = creature.gameObject.AddChildWithComponent<ResourcePile>(
				ResourcesPathfinder.ResourcePilePrefab());
			var translate = creature.GetComponentInChildren<MeshRenderer>().bounds.size.y;
			ability.resourcePile.transform.Translate(Vector3.up * translate);
			return ability;
		}
	}

	public override string Description()
	{
		if (Carrying.IsEmpty())
		{
			return "Pick up " + capacity + " items";
		}
		else
		{
			return "Drop off " + Carrying.Count() + " items";
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
		// Update the visual representation
		resourcePile.ResourceCollection = Carrying;
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
