using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class RecipeManager : MonoBehaviour
{

	// the element to use to determine where to create creatures
	public GameObject createMarker;

	// The location of all the instructions in the world
	private IDictionary<Coordinate, Recipe> recipeLocations;
	// The list of available instructions in the game
	private IList<CreatureType> availableRecipes;

	// True if we are currently creating a new creature
	private bool isCreating = false;

	public bool IsCreating
	{
		get { return isCreating; }
		set
		{
			isCreating = value;
			if (createMarker) { createMarker.SetActive(isCreating); }
		}
	}

	// Use this for initialization
	void Start ()
	{
		recipeLocations = new Dictionary<Coordinate, Recipe>();
		availableRecipes = new List<CreatureType>();


		foreach (var instruction in GetComponentsInChildren<Recipe>())
		{
			recipeLocations[instruction.Coordinate] = instruction;
		}
	}

	// Update the list of available instructions if a creature has walked on it
	public void UpdateAvailableRecipes(ICollection<Coordinate> creaturePositions)
	{
		var removedEntries = new List<Coordinate>();
		foreach (var entry in recipeLocations)
		{
			// If there is a creature at our location, remove this instruction
			// from the grid and add it to the list of available instructions
			if (creaturePositions.Contains(entry.Key))
			{
				if (!availableRecipes.Contains(entry.Value.creature))
				{
					availableRecipes.Add(entry.Value.creature);
				}
				removedEntries.Add(entry.Key);
			}
		}
		foreach (var coord in removedEntries)
		{
			Destroy(recipeLocations[coord].gameObject);
			recipeLocations.Remove(coord);
		}
	}

	public void OnMouseOverTerrain(Coordinate coordinate)
	{
		if (createMarker != null && isCreating)
		{
			// Update the position visually
			GameManager.gm.SetPosition(createMarker, coordinate);
		}
	}

	public void OnMouseDownTerrain(Coordinate coordinate)
	{
		if (createMarker != null && isCreating)
		{
			// Figure out how many blocks we have available
			// TODO do a type alias of the multiset.
			IEnumerable<IDictionary<ResourceType, int>> availableResources = Neighbors(coordinate)
				.Select(c => GameManager.gm.resources.Contains(c) ? GameManager.gm.resources[c] : new Dictionary<ResourceType, int>());
			IDictionary<ResourceType, int> resourceCount = new Dictionary<ResourceType, int>();
			foreach (var resource in availableResources)
			{
				resourceCount = Add(resourceCount, resource);
			}
			Debug.LogFormat("Calculated resources: {0}", String.Join("; ", resourceCount.Select(e => e.Key + ": " + e.Value).ToArray()));

			// TODO check if we have enough resources for the recipe

			if (Contains(resourceCount, Recipes.dragonRecipe))
			{
				// If everything passes, add the creature to the list of creatures
				GameManager.gm.creatures.AddCreature(CreatureType.Dragon, coordinate);
			}
			else
			{
				Debug.LogFormat("Did not find enough materials for recipe. Had {0}, needed {1}",
					resourceCount, Recipes.dragonRecipe);
			}


			// TODO subtract the blocks needed from the resource manager

		}
	}

	public void Update()
	{
		// For now, let's go into the active state when we don't have anything selected.
		// TODO get rid of this and only call this when a UI button is selected.
		if (Input.GetKeyDown("space") && (GameManager.gm.creatures.SelectedCreature == null))
		{
			IsCreating = !IsCreating;
		}
	}

	// Adds up the two multisets into a third multiset.
	// TODO put this in an extension method or something
	private static IDictionary<T, int> Add<T>(IDictionary<T, int> first, IDictionary<T, int> second)
	{
		var result = new Dictionary<T, int>(first);
		foreach (var entry in second)
		{
			if (result.ContainsKey(entry.Key))
			{
				result[entry.Key] += entry.Value;
			}
			else
			{
				result[entry.Key] = entry.Value;
			}
		}
		return result;
	}

	// Check if one multiset contains another
	private static bool Contains<T>(IDictionary<T, int> superset, IDictionary<T, int> subset)
	{
		return subset.All(entry => superset.ContainsKey(entry.Key) && superset[entry.Key] >= entry.Value);
	}

	private static IList<Coordinate> Neighbors(Coordinate coordinate)
	{
		int[] range = {-1, 0, 1};
		return (from x in range
			from z in range
			select coordinate + new Coordinate(x, z)).ToList();

	}

}

// TODO move this to a separate file
public static class Recipes
{
	public static IDictionary<ResourceType, int> dragonRecipe = new Dictionary<ResourceType, int>()
	{
		{ResourceType.Red, 5}
	};

	public static IDictionary<ResourceType, int> duckRecipe = new Dictionary<ResourceType, int>()
	{
		{ResourceType.Yellow, 1}
	};
}