using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;


public class RecipeController : MonoBehaviour
{

	// The location of all the instructions in the world
	private IDictionary<Coordinate, Recipe> recipeLocations;
	// The list of available instructions in the game
	private IList<CreatureType> availableRecipes;

	public Action<IList<CreatureType>> OnChange;

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
		if (removedEntries.Count > 0)
		{
			if (OnChange != null) { OnChange(availableRecipes); }
		}
	}

}
