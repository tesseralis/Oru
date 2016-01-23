using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

/// <summary>
/// Manages state specific to individual levels: the terrain layout, the set of
/// creatures, resources, and recipes, and the win conditions of each level.
/// 
/// Also manages the internal game clock, putting it into discrete steps.
/// </summary>
public class LevelManager : MonoBehaviour
{
	public static LevelManager level;
	public static string levelName;

	public static TerrainController Terrain { get { return level.terrainController; } }
	public static CreatureController Creatures { get { return level.creatureController; } }
	public static ResourceController Resources { get { return level.resourceController; } }
	public static RecipeController Recipes { get { return level.recipeController; } }
	public static GoalController Goals { get { return level.goalController; } }

	/*
	 * Public members that can be set per level.
	 */
	// the grid of available game blocks
	public TerrainController terrainController;
	// the creatures in the game world
	public CreatureController creatureController;
	// the resources in the game world
	public ResourceController resourceController;
	// the recipes of the game world
	public RecipeController recipeController;
	// the goals of the game world
	public GoalController goalController;
	
	// width of the space between individual cells
	// TODO move this to an option in a custom inspector
	public const float cellSize = 2;

	void Awake ()
	{
		if (level == null) { level = this; }

		// Auto-wire the controllers if necessary
		if (!creatureController) { creatureController = GetComponentInChildren<CreatureController>(); }
		if (!terrainController) { terrainController = GetComponentInChildren<TerrainController>(); }
		if (!resourceController) { resourceController = GetComponentInChildren<ResourceController>(); }
		if (!recipeController) { recipeController = GetComponentInChildren<RecipeController>(); }
		if (!goalController) { goalController = GetComponentInChildren<GoalController>(); }

		if (levelName != null)
		{
			Serialization.DeserializeLevel(levelName);
		}
	}

	public static void SetLevel(string _levelName)
	{
		levelName = _levelName;
		if (level) { Serialization.DeserializeLevel(levelName); }
	}

}
