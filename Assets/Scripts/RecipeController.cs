using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;


public class RecipeController : MonoBehaviour
{

	// the element to use to determine where to create creatures
	public GameObject createMarker;

	// The location of all the instructions in the world
	private IDictionary<Coordinate, Recipe> recipeLocations;
	// The list of available instructions in the game
	private IList<CreatureType> availableRecipes;

	public IList<CreatureType> AvailableRecipes
	{
		get { return availableRecipes; }
	}

	// True if we are currently creating a new creature
	private bool isCreating = false;

	public bool IsCreating
	{
		get { return isCreating; }
		set
		{
			isCreating = value;
			// If we're turning "on", disable all other UI.
			if (isCreating)
			{
				// TODO another space where we'd benefit from a "deselect all" method
				GameManager.Creatures.SelectedCreature = null;
				GameManager.Creatures.IsActing = false;
			}
			if (createMarker) { createMarker.SetActive(isCreating); }
		}
	}

	public CreatureType CurrentRecipe { get; set; }

	// Use this for initialization
	void Start ()
	{
		recipeLocations = new Dictionary<Coordinate, Recipe>();
		availableRecipes = new List<CreatureType>();


		foreach (var instruction in GetComponentsInChildren<Recipe>())
		{
			recipeLocations[instruction.Coordinate()] = instruction;
		}

		// Set event handlers
		// TODO add and remove these handlers based on state.
		GameManager.Terrain.OnHover += MoveCreateMarker;
		GameManager.Terrain.OnClick += CreateCreature;
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

	void MoveCreateMarker(Coordinate coordinate)
	{
		if (createMarker != null && isCreating)
		{
			// Update the position visually
			GameManager.gm.SetPosition(createMarker, coordinate);
		}
	}

	void CreateCreature(Coordinate coordinate)
	{
		var resources = GameManager.Resources;
		if (createMarker != null && isCreating)
		{
			var recipe = Creatures.ForType(CurrentRecipe).Recipe;
			// Figure out how many blocks we have available
			var availableResources = Neighbors(coordinate).Select(c => resources[c]);
			var resourceCount = Multiset.Empty<ResourceType>();
			foreach (var resource in availableResources)
			{
				resourceCount = resourceCount.MultisetAdd(resource);
			}
			Debug.LogFormat("Calculated resources: {0}", String.Join("; ", resourceCount.Select(e => e.Key + ": " + e.Value).ToArray()));

			if (resourceCount.Contains(recipe))
			{
				// If everything passes, add the creature to the list of creatures
				GameManager.Creatures.AddCreature(CurrentRecipe, coordinate);

				// Remove the items from the neighboring coordinates.
				var neighbors = Neighbors(coordinate);
				var remainder = recipe;
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
				// We are no longer creating
				IsCreating = false;
			}
			else
			{
				Debug.LogFormat("Did not find enough materials for recipe. Had {0}, needed {1}",
					resourceCount, recipe);
			}


			// TODO subtract the blocks needed from the resource manager

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
