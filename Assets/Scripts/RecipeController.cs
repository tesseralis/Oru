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
		if (createMarker != null && isCreating)
		{
			if (GameManager.Creatures.CanCreateCreature(CurrentRecipe, coordinate))
			{
				// If everything passes, add the creature to the list of creatures
				GameManager.Creatures.CreateCreature(CurrentRecipe, coordinate);
			}
		}
		// We are no longer creating
		IsCreating = false;
	}

}
