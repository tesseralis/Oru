using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.RepresentationModel;
using Util;

public static class Deserializer
{
	// Deserialize coordinates in the form x,z
	private static Coordinate DeserializeCoordinate(YamlNode node)
	{
		int[] coords = node.ToString().Split(',').Select(x => Int32.Parse(x)).ToArray();
		return new Coordinate(coords[0], coords[1]);
	}

	public static IDictionary<Coordinate, T> DeserializeCoordinateMap<T>(YamlMappingNode map, Func<YamlNode, T> func)
	{
		return map.ToDictionary<Coordinate, T>(DeserializeCoordinate, func);
	}

	private static TerrainType DeserializeTerrainTile(char chr)
	{
		switch(chr)
		{
		case 'l': return TerrainType.Land;
		case 'w': return TerrainType.Water;
		case 'r': return TerrainType.Rock;
		case 't': return TerrainType.Tree;
		default: throw new ArgumentException("Illegal terrain type");
		}
	}

	private static IDictionary<Coordinate, TerrainType> DeserializeTerrain(string terrainString)
	{
		var terrainGrid = new Dictionary<Coordinate, TerrainType>();
		var terrain = terrainString.Split('\n');
		for (int i = 0; i < terrain.Length; i++)
		{
			for (int j = 0; j < terrain[i].Length; j++)
			{
				if (terrain[i][j] != ' ')
				{
					var type = DeserializeTerrainTile(terrain[i][j]);
					var coordinate = new Coordinate(i, j);
					terrainGrid[coordinate] = type;
				}
			}
		}
		return terrainGrid;
	}

	private static ResourceCollection DeserializeResourceCollection(YamlNode node)
	{
		var dict = node.ToDictionary<ResourceType, int>(YamlExtensions.ToEnum<ResourceType>, YamlExtensions.ToInt);
		return ResourceCollection.FromMultiset(dict);
	}

	public static void DeserializeLevel(string levelName)
	{
		var levelFile = UnityEngine.Resources.Load<TextAsset>("Levels/" + levelName);
		Debug.Log(levelFile.text);
		var input = new StringReader(levelFile.text);
		var yaml = new YamlStream();
		yaml.Load(input);

		var levelMapping = yaml.Documents[0].RootNode.AsMapping();

		var terrain = GameObject.Find("Terrain").GetComponent<TerrainController>();
		terrain.gameObject.DestroyAllChildrenImmediate();
		foreach (var entry in DeserializeTerrain(levelMapping.GetString("terrain")))
		{
			terrain.AddTerrainTile(entry.Key, entry.Value);
		}

		var creatures = GameObject.Find("Creatures").GetComponent<CreatureController>();
		creatures.gameObject.DestroyAllChildrenImmediate();
		var creatureMapping = DeserializeCoordinateMap(levelMapping.GetMapping("creatures"), x => x.ToEnum<CreatureType>());
		foreach (var entry in creatureMapping)
		{
			creatures.AddCreature(entry.Key, entry.Value);

		}

		var resources = GameObject.Find("Resources").GetComponent<ResourceController>();
		resources.gameObject.DestroyAllChildrenImmediate();
		var resourcesMapping = DeserializeCoordinateMap(levelMapping.GetMapping("resources"), x => DeserializeResourceCollection(x));
		foreach (var entry in resourcesMapping)
		{
			resources.AddResourcePile(entry.Key, entry.Value);
		}

		var recipes = GameObject.Find("Recipes").GetComponent<RecipeController>();
		recipes.gameObject.DestroyAllChildrenImmediate();
		var recipeMapping = levelMapping.GetMapping("recipes");

		// TODO this won't work when loading a level from the level editor
		// I think it has to do with setting dirty flags
		var available = recipeMapping.GetSequence("available").Select(x => x.ToEnum<CreatureType>()).ToArray();
		recipes.availableRecipes = available;

		var field = DeserializeCoordinateMap(recipeMapping.GetMapping("field"), x => x.ToEnum<CreatureType>());
		foreach (var entry in field)
		{
			recipes.AddRecipe(entry.Key, entry.Value);
		}

		var goals = GameObject.Find("Goals").GetComponent<GoalController>();
		goals.gameObject.DestroyAllChildrenImmediate();
		var goalMapping = DeserializeCoordinateMap(levelMapping.GetMapping("goals"), x => x.ToEnum<CreatureType>());
		foreach (var goal in goalMapping)
		{
			goals.AddGoal(goal.Key, goal.Value);
		}
	}


	public static IList<String> DeserializeLevelList()
	{
		var levelListFile = UnityEngine.Resources.Load<TextAsset>("level-list");
		var input = new StringReader(levelListFile.text);
		var yaml = new YamlStream();
		yaml.Load(input);
		return yaml.Documents[0].RootNode.AsSequence().Select(x => x.ToString()).ToList();
	}
}