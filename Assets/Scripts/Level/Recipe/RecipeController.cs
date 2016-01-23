using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;


public class RecipeController : MonoBehaviour
{
	// Prefab to use for creating recipes
	public GameObject recipePrefab;

	// The list of recipes available to the player
	public CreatureType[] availableRecipes;

	// The location of all the instructions in the world
	private IDictionary<Coordinate, Recipe> recipeLocations;

	public event Action<IList<CreatureType>> RecipesUpdated;

	public CreatureType CurrentRecipe { get; set; }

	public CreatureType? this[Coordinate coordinate]
	{
		get
		{
			if (recipeLocations.ContainsKey(coordinate))
			{
				return recipeLocations[coordinate].creature;
			}
			else
			{
				return null;
			}
		}
		set
		{
			if (value.HasValue)
			{
				if (!recipeLocations.ContainsKey(coordinate))
				{
					recipeLocations[coordinate] = AddRecipe(coordinate, value.Value);
				}
				else
				{
					recipeLocations[coordinate].creature = value.Value;
				}
			}
			else
			{
				if (recipeLocations.ContainsKey(coordinate))
				{
					var removed = recipeLocations[coordinate];
					recipeLocations.Remove(coordinate);
					Destroy(removed.gameObject);
				}
			}
		}
	}

	// Add a recipe to this game object, but do not update the game state
	public Recipe AddRecipe(Coordinate coordinate, CreatureType type)
	{
		var prefab = ResourcesPathfinder.RecipePrefab();
		var recipe = gameObject.AddChildWithComponent<Recipe>(prefab, coordinate);
		recipe.creature = type;
		return recipe;
	}

	public IList<CreatureType> AvailableRecipes
	{
		get
		{
			return availableRecipes.ToList();
		}
		set
		{
			availableRecipes = value.ToArray();
			if (RecipesUpdated != null) { RecipesUpdated(availableRecipes); }
		}
	}

	void Awake ()
	{
		recipeLocations = new Dictionary<Coordinate, Recipe>();
		
		if (availableRecipes == null)
		{
			availableRecipes = new CreatureType[0];
		}
	}

	void Start()
	{
		foreach (var instruction in GetComponentsInChildren<Recipe>())
		{
			recipeLocations[instruction.gameObject.Coordinate()] = instruction;
		}
		LevelManager.Creatures.CreaturesUpdated += UpdateAvailableRecipes;
	}

	// Update the list of available instructions if a creature has walked on it
	private void UpdateAvailableRecipes(IList<Creature> creatureList)
	{
		var creaturePositions = creatureList.Select(x => x.Position).ToList();
		var removedEntries = new List<Coordinate>();
		foreach (var entry in recipeLocations)
		{
			// If there is a creature at our location, remove this instruction
			// from the grid and add it to the list of available instructions
			if (creaturePositions.Contains(entry.Key))
			{
				if (!AvailableRecipes.Contains(entry.Value.creature))
				{
					AvailableRecipes = AvailableRecipes.Union(new CreatureType[] { entry.Value.creature }).ToList();
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

}
