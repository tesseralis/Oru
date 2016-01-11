using UnityEngine;
using System;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.RepresentationModel;
using Util;

public static class Deserializer
{
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

	public static void DeserializeLevel(string levelName)
	{
		var levelFile = UnityEngine.Resources.Load<TextAsset>("Levels/" + levelName);
		Debug.Log(levelFile.text);
		var input = new StringReader(levelFile.text);
		var yaml = new YamlStream();
		yaml.Load(input);

		var level = (YamlMappingNode)yaml.Documents[0].RootNode;

		// TODO factor out terrain deserialization
		var terrain = level.GetString("terrain").Split('\n');
		for (int i = 0; i < terrain.Length; i++)
		{
			for (int j = 0; j < terrain[i].Length; j++)
			{
				if (terrain[i][j] != ' ')
				{
					var type = DeserializeTerrainTile(terrain[i][j]);
					var coordinate = new Coordinate(i, j);
					Debug.LogFormat("Set {0}: to {1}", coordinate, type);
					LevelManager.Terrain[coordinate] = type;
				}
			}
		}

		var creatures = level.GetMapping("creatures");
		foreach (var entry in creatures.ToCoordinateMap<CreatureType>(YamlExtensions.ToEnum<CreatureType>))
		{
			Debug.Log(entry.Key + " " + entry.Value);
			LevelManager.Creatures.AddCreature(entry.Value, entry.Key);
		}

		var resourcePiles = level.GetMapping("resources");
		foreach (var entry in resourcePiles.ToCoordinateMap(x => ((YamlMappingNode)x).ToDictionary<ResourceType, int>(YamlExtensions.ToEnum<ResourceType>, YamlExtensions.ToInt)))
		{
			Debug.Log(entry.Key + " " + entry.Value);
			LevelManager.Resources[entry.Key] = ResourceCollection.FromMultiset(entry.Value);
		}

		var recipes = level.GetMapping("recipes");
		var available = recipes.GetSequence("available");
		LevelManager.Recipes.AvailableRecipes = available.Select<YamlNode, CreatureType>(YamlExtensions.ToEnum<CreatureType>).ToArray();
		var field = recipes.GetMapping("field").ToCoordinateMap<CreatureType>(YamlExtensions.ToEnum<CreatureType>);
		foreach (var entry in field)
		{
			LevelManager.Recipes[entry.Key] = entry.Value;
		}

		var goals = level.GetMapping("goals").ToCoordinateMap<CreatureType>(YamlExtensions.ToEnum<CreatureType>);
		foreach (var goal in goals)
		{
			LevelManager.Goals.SetGoal(goal.Key, goal.Value);
		}
	}

}