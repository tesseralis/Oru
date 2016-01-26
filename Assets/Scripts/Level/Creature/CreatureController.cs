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
	// Maximum health a creature can have
	// TODO Move this to a custom editor window
	public const int maxHealth = 50;

	// Called when a creature is destroyed
	public event Action<Creature, Coordinate> CreatureDestroyed;
	// Called when a creature is created
	public event Action<Creature, Coordinate> CreatureCreated;
	// Called when a single creature has updated its step
	public event Action<Creature> CreatureStepped;
	// TODO I can probably refactor this out because of the above method
	// Called when all the creatures have updated their steps
	public event Action<IList<Creature>> CreaturesUpdated;

	private IList<Creature> creaturesToDestroy = new List<Creature>();

	public int Steps { get; private set; }

	public IList<Creature> CreatureList { get; private set; }

	public Creature this[Coordinate position]
	{
		get { return CreatureList.FirstOrDefault(x => x.Position == position); }
	}

	// Add a creature at the specified location, but do not add it to the game state
	public Creature AddCreature(Coordinate location, CreatureType creatureType)
	{
		var prefab = ResourcesPathfinder.CreaturePrefab(creatureType);
		var newCreature = gameObject.AddChildWithComponent<Creature>(prefab, location);
		newCreature.creatureType = creatureType;
		return newCreature;
	}

	void Start()
	{
		// Add all the creatures we have on the board right now
		CreatureList = GetComponentsInChildren<Creature>().ToList();

		// Creatures should all start out with full health
		foreach (var creature in CreatureList)
		{
			creature.health = CreatureController.maxHealth;
		}
	}

	// Returns true if we can add a creature at the given coordinate
	public bool CanCreateCreature(CreatureType creature, Coordinate coordinate)
	{
		var creatureDefinition = CreatureDefinition.ForType(creature);
		var resources = LevelManager.Resources;
		var recipe = creatureDefinition.Recipe;
		// Figure out how many blocks we have available
		var availableResources = Neighbors(coordinate).Select(c => resources[c]);
		var resourceCount = availableResources.Aggregate((x, y) => x + y);
		return resourceCount.ToMultiset().Contains(recipe)
			&& creatureDefinition.AllowedTerrain.Contains(LevelManager.Terrain[coordinate])
			&& !CreatureList.Any(x => x.Position == coordinate);
	}


	// Add a creature at a specified location if possible
	public Creature CreateCreature(CreatureType creature, Coordinate location)
	{
		var definition = CreatureDefinition.ForType(creature);
		if (!CanCreateCreature(creature, location))
		{
			throw new ArgumentException(string.Format("Cannot create {0} at {1}.",
				creature, location));
		}
		var resources = LevelManager.Resources;

		// Remove the items from the neighboring coordinates.
		var neighbors = Neighbors(location);
		// If the creature needs an energy resource, take it from the most healthy pile
		int bestEnergy = 0;
		if (!definition.NoEnergy)
		{
			bestEnergy = neighbors.SelectMany(x => LevelManager.Resources[x].EnergyBlocks).Max();
			// Remove from the thing we took it from
			foreach (var neighbor in neighbors)
			{
				if (resources[neighbor].EnergyBlocks.Contains(bestEnergy))
				{
					resources[neighbor] = resources[neighbor].Remove(bestEnergy);
					break;
				}
			}
		}
		else
		{
			bestEnergy = maxHealth;
		}
		// Take items from the adjacent resources until we don't need any more.
		var remainder = definition.Recipe;
		foreach (var neighbor in neighbors)
		{
			if (remainder.IsEmpty())
			{
				break;
			}
			var difference = resources[neighbor].Paper.MultisetSubtract(remainder);
			remainder = remainder.MultisetSubtract(resources[neighbor].Paper);

			resources[neighbor] = resources[neighbor].WithPaper(difference);
		}
		var newCreature = AddCreature(location, creature);
		newCreature.health = bestEnergy;
		CreatureList.Add(newCreature);


		// Call any necessary events
		if (CreatureCreated != null) { CreatureCreated(newCreature, location); }

		return newCreature;
	}

	public void DestroyCreature(Creature creature)
	{
		creaturesToDestroy.Add(creature);
	}

	private void DoDestroyCreature(Creature creature)
	{
		var coordinate = creature.Position;

		// Answer any event handlers
		if (CreatureDestroyed != null) { CreatureDestroyed(creature, coordinate); }

		// Recycle the creature's components
		LevelManager.Resources[coordinate] += creature.ToResources();

		// Do anything the creature's ability's state says we should do.
		// TODO generalize this so that we can account for more abilities
		if (creature.GetComponent<CarryResourceAbility>() != null)
		{
			var ability = creature.GetComponent<CarryResourceAbility>();
			LevelManager.Resources[coordinate] += ability.Carrying;
		}

		// Remove from our list of creatures
		CreatureList.Remove(creature);

		// Remove from the hierarchy
		Destroy(creature.gameObject);
	}

	/// <summary>
	/// Move all the creatures forward one game step.
	/// </summary>
	public void StepCreatures()
	{
		Steps++;
		foreach(Creature creature in CreatureList)
		{
			creature.Step();
			if (CreatureStepped != null) { CreatureStepped(creature); }
		}
		// Remove creatures that have died
		var deadCreatures = CreatureList.Where(x => x.health < 0).ToList();
		foreach (var creature in deadCreatures)
		{
			// Enemy creatures give out max health
			creature.health = creature.Definition.IsEnemy ? CreatureController.maxHealth : 0;
		}
		// Destroy all dead creatures and creatures the user wants to destroy
		foreach (var creature in deadCreatures.Union(creaturesToDestroy))
		{
			DoDestroyCreature(creature);
		}
		creaturesToDestroy.Clear();
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
