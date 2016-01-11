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

		var level = (YamlMappingNode)yaml.Documents[0].RootNode;

		foreach (var entry in DeserializeTerrain(level.GetString("terrain")))
		{
			LevelManager.Terrain[entry.Key] = entry.Value;
		}

		var creatures = DeserializeCoordinateMap(level.GetMapping("creatures"), x => x.ToEnum<CreatureType>());
		foreach (var entry in creatures)
		{
			LevelManager.Creatures.AddCreature(entry.Value, entry.Key);
		}

		var resources = DeserializeCoordinateMap(level.GetMapping("resources"), x => DeserializeResourceCollection(x));
		foreach (var entry in resources)
		{
			LevelManager.Resources[entry.Key] = entry.Value;
		}

		var recipes = level.GetMapping("recipes");
		var available = recipes.GetSequence("available").Select(x => x.ToEnum<CreatureType>()).ToList();
		LevelManager.Recipes.AvailableRecipes = available;

		var field = DeserializeCoordinateMap(recipes.GetMapping("field"), x => x.ToEnum<CreatureType>());
		foreach (var entry in field)
		{
			LevelManager.Recipes[entry.Key] = entry.Value;
		}

		var goals = DeserializeCoordinateMap(level.GetMapping("goals"), x => x.ToEnum<CreatureType>());
		foreach (var goal in goals)
		{
			LevelManager.Goals.SetGoal(goal.Key, goal.Value);
		}
	}

}