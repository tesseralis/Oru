using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

/// <summary>
/// Manages interactions between creatures and the rest of the system.
/// </summary>
public class CreatureController : MonoBehaviour
{

	public CreaturePrefabOptions creaturePrefabs;

	public Action<Creature> OnSelect;

	public event Action<IList<Creature>> UpdateCreatures;

	public IList<Creature> CreatureList
	{
		get
		{
			return new List<Creature>(GetComponentsInChildren<Creature>());
		}
	}

	void Start()
	{
		GameManager.gm.Step += StepCreatures;
	}

	// Returns true if we can add a creature at the given coordinate
	public bool CanCreateCreature(CreatureType creature, Coordinate coordinate)
	{
		var creatureDefinition = Creatures.ForType(creature);
		var resources = GameManager.Resources;
		var recipe = creatureDefinition.Recipe;
		// Figure out how many blocks we have available
		var availableResources = Neighbors(coordinate).Select(c => resources[c]);
		var resourceCount = Multiset.Empty<ResourceType>();
		foreach (var resource in availableResources)
		{
			resourceCount = resourceCount.MultisetAdd(resource);
		}
		return resourceCount.Contains(recipe)
			&& creatureDefinition.AllowedTerrain.Contains(GameManager.Terrain[coordinate])
			&& !CreatureList.Any(x => x.Position == coordinate);

	}

	// Add a creature at a specified location if possible
	public Creature CreateCreature(CreatureType creature, Coordinate location)
	{
		if (!CanCreateCreature(creature, location))
		{
			throw new ArgumentException(string.Format("Cannot create {0} at {1}.",
				creature, location));
		}
		var resources = GameManager.Resources;

		// Remove the items from the neighboring coordinates.
		var neighbors = Neighbors(location);
		var remainder = Creatures.ForType(creature).Recipe;
		// Take items from the adjacent resources until we don't need any more.
		foreach (var neighbor in neighbors)
		{
			if (remainder.IsEmpty())
			{
				break;
			}
			var difference = GameManager.Resources[neighbor].MultisetSubtract(remainder);
			remainder = remainder.MultisetSubtract(resources[neighbor]);

			resources[neighbor] = difference;
		}
		// TODO sync this up with the steps so we don't accidentally win ahead of time
		return gameObject.AddChildWithComponent<Creature>(creaturePrefabs.PrefabFor (creature), location);
	}

	public void DestroyCreature(Creature creature)
	{
		var coordinate = creature.Position;

		// Recycle the creature's components
		GameManager.Resources[coordinate] = GameManager.Resources[coordinate].MultisetAdd(Creatures.ForType(creature.creatureType).Recipe);

		// Do anything the creature's ability's state says we should do.
		// TODO generalize this so that we can account for more abilities
		if (creature.GetComponent<CarryResourceAbility>() != null)
		{
			var ability = creature.GetComponent<CarryResourceAbility>();
			GameManager.Resources[coordinate] = GameManager.Resources[coordinate].MultisetAdd(ability.Carrying);
		}

		// Remove from the creature list
		Destroy(creature.gameObject);
	}

	/// <summary>
	/// Move all the creatures forward one game step.
	/// </summary>
	void StepCreatures()
	{
		foreach(Creature creature in CreatureList)
		{
			creature.Step();
		}
		if (UpdateCreatures != null) { UpdateCreatures(CreatureList); }
	}
		
	private static IList<Coordinate> Neighbors(Coordinate coordinate)
	{
		int[] range = {-1, 0, 1};
		return (from x in range
			from z in range
			select coordinate + new Coordinate(x, z)).ToList();

	}
}

[Serializable]
public class CreaturePrefabOptions
{
	public GameObject cranePrefab;
	public GameObject turtlePrefab;
	public GameObject horsePrefab;
	public GameObject elephantPrefab;

	public GameObject PrefabFor (CreatureType creature)
	{
		switch (creature) {
		case CreatureType.Crane: return cranePrefab;
		case CreatureType.Turtle: return turtlePrefab;
		case CreatureType.Horse: return horsePrefab;
		case CreatureType.Elephant: return elephantPrefab;
		default: throw new ArgumentException("Illegal creature type: " + creature, "creature");
		}
	}
}
