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

	public IList<Creature> CreatureList
	{
		get
		{
			return new List<Creature>(GetComponentsInChildren<Creature>());
		}
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
			&& creatureDefinition.AllowedTerrain.Contains(GameManager.Terrain[coordinate]);

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

		return this.AddChildWithComponent<Creature>(PrefabFor (creature), location);

	}

	/// <summary>
	/// Move all the creatures forward one game step.
	/// </summary>
	public void Step()
	{
		foreach(Creature creature in CreatureList)
		{
			creature.Step();
		}
	}

	public GameObject PrefabFor (CreatureType creature)
	{
		switch (creature) {
		case CreatureType.Duck:
			return creaturePrefabs.duckPrefab;
		case CreatureType.Dragon:
			return creaturePrefabs.dragonPrefab;
		default:
			throw new ArgumentException("Illegal creature type", creature.ToString());
		}
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
	public GameObject duckPrefab;
	public GameObject dragonPrefab;
}
