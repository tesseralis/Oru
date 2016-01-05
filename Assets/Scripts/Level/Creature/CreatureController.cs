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

	// Called when a creature is destroyed
	public event Action<Coordinate> CreatureDestroyed;
	// Called when a creature is created
	public event Action<Creature, Coordinate> CreatureCreated;
	// Called when all the creatures have updated their steps
	public event Action<IList<Creature>> CreaturesUpdated;

	private IList<Creature> creatureList;

	public IList<Creature> CreatureList
	{
		get { return creatureList; }
	}

	void Start()
	{
		// Update all our creatures when we tick
		LevelManager.level.Step += StepCreatures;

		// Add all the creatures we have on the board right now
		creatureList = GetComponentsInChildren<Creature>().ToList();
	}

	// Returns true if we can add a creature at the given coordinate
	public bool CanCreateCreature(CreatureType creature, Coordinate coordinate)
	{
		var creatureDefinition = CreatureDefinitions.ForType(creature);
		var resources = LevelManager.Resources;
		var recipe = creatureDefinition.Recipe;
		// Figure out how many blocks we have available
		var availableResources = Neighbors(coordinate).Select(c => resources[c]);
		var resourceCount = Multiset.Empty<ResourceType>();
		foreach (var resource in availableResources)
		{
			resourceCount = resourceCount.MultisetAdd(resource);
		}
		return resourceCount.Contains(recipe)
			&& creatureDefinition.AllowedTerrain.Contains(LevelManager.Terrain[coordinate])
			&& !CreatureList.Any(x => x.Position == coordinate);

	}

	// Add a creature at a specified location if possible
	public Creature CreateCreature(CreatureType creature, Coordinate location)
	{
		var definition = CreatureDefinitions.ForType(creature);
		if (!CanCreateCreature(creature, location))
		{
			throw new ArgumentException(string.Format("Cannot create {0} at {1}.",
				creature, location));
		}
		var resources = LevelManager.Resources;

		// Remove the items from the neighboring coordinates.
		var neighbors = Neighbors(location);
		var remainder = definition.Recipe;
		// Take items from the adjacent resources until we don't need any more.
		foreach (var neighbor in neighbors)
		{
			if (remainder.IsEmpty())
			{
				break;
			}
			var difference = LevelManager.Resources[neighbor].MultisetSubtract(remainder);
			remainder = remainder.MultisetSubtract(resources[neighbor]);

			resources[neighbor] = difference;
		}
		var newCreature = gameObject.AddChildWithComponent<Creature>(creaturePrefabs.PrefabFor (creature), location);

		// Add the creature to our list
		creatureList.Add(newCreature);

		// Call any necessary events
		if (CreatureCreated != null) { CreatureCreated(newCreature, location); }

		return newCreature;
	}

	// TODO figure out why this creates stray GameObjects
	public void DestroyCreature(Creature creature)
	{
		var coordinate = creature.Position;

		// Answer any event handlers
		if (CreatureDestroyed != null) { CreatureDestroyed(coordinate); }

		// Recycle the creature's components
		LevelManager.Resources[coordinate] = LevelManager.Resources[coordinate].MultisetAdd(CreatureDefinitions.ForType(creature.creatureType).Recipe);

		// Do anything the creature's ability's state says we should do.
		// TODO generalize this so that we can account for more abilities
		if (creature.GetComponent<CarryResourceAbility>() != null)
		{
			var ability = creature.GetComponent<CarryResourceAbility>();
			LevelManager.Resources[coordinate] = LevelManager.Resources[coordinate].MultisetAdd(ability.Carrying);
		}

		// Remove from our list of creatures
		creatureList.Remove(creature);

		// Remove from the hierarchy
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
		if (CreaturesUpdated != null) { CreaturesUpdated(CreatureList); }
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
