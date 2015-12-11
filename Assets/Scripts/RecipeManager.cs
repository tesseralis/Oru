using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class RecipeManager : MonoBehaviour
{

	// the element to use to determine where to create creatures
	public GameObject createMarker;

	public RecipeListEntry[] recipeList;

	// The location of all the instructions in the world
	private IDictionary<Coordinate, Recipe> recipeLocations;
	// The list of available instructions in the game
	private IList<CreatureType> availableRecipes;

	// True if we are currently creating a new creature
	private bool isCreating = false;

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
			// TODO use the whole grid of nine blocks

			// TODO un-hardcode the recipe we need
			var currentRecipe = new Dictionary<ResourceType, int>();
			currentRecipe[ResourceType.Red] = 3; // we need three red for a dragon

			// TODO link the Dragon prefab from somewhere as part of the current recipe

			// TODO check if we have enough resources for the recipe

			// TODO if so, create a new instance of the creature in the prefab


		}
	}

	public void Update()
	{
		// For now, let's go into the active state when we don't have anything selected.
		// TODO get rid of this and only call this when a UI button is selected.
		if (Input.GetKeyDown("space") && (GameManager.gm.creatures.SelectedCreature == null))
		{
			isCreating = !isCreating;
			createMarker.SetActive(isCreating);
		}
	}

}

// TODO does this make sense as something included in the "recipe" class instead?
[Serializable]
public class RecipeListEntry
{
	public CreatureType type;
	public ResourceCount[] recipe;
	public GameObject prefab;
}