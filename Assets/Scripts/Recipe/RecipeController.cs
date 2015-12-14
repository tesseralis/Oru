﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;


public class RecipeController : MonoBehaviour
{
	// The list of recipes available to the player
	public CreatureType[] availableRecipes;

	// The location of all the instructions in the world
	private IDictionary<Coordinate, Recipe> recipeLocations;

	public Action<IList<CreatureType>> OnChange;

	public CreatureType CurrentRecipe { get; set; }

	void Awake ()
	{
		recipeLocations = new Dictionary<Coordinate, Recipe>();
		
		if (availableRecipes == null)
		{
			availableRecipes = new CreatureType[0];
		}
		
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
					availableRecipes = availableRecipes.Union(new CreatureType[] { entry.Value.creature }).ToArray();
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