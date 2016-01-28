using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.RepresentationModel;
using Util;

public static class Serialization
{
	private static CreatureType DeserializeCreatureType(YamlNode node)
	{
		return new CreatureType(node.ToString());
	}

	// Deserialize coordinates in the form x,z
	private static Coordinate DeserializeCoordinate(YamlNode node)
	{
		int[] coords = node.ToString().Split(',').Select(x => Int32.Parse(x)).ToArray();
		return new Coordinate(coords[0], coords[1]);
	}

	private static YamlNode SerializeCoordinate(Coordinate coordinate)
	{
		return new YamlScalarNode(string.Format("{0},{1}", coordinate.x, coordinate.z));
	}

	private static IDictionary<Coordinate, T> DeserializeCoordinateMap<T>(YamlMappingNode map, Func<YamlNode, T> func)
	{
		return map.ToDictionary<Coordinate, T>(DeserializeCoordinate, func);
	}

	private static YamlNode SerializeCoordinateMap<T>(GameObject gameObject, Func<T, YamlNode> func) where T : MonoBehaviour
	{
		return new YamlMappingNode(gameObject.GetComponentsInChildren<T>().Select(x =>
			new KeyValuePair<YamlNode, YamlNode>(SerializeCoordinate(x.gameObject.Coordinate()), func(x))));
	}

	private static TerrainType DeserializeTerrainTile(char chr)
	{
		switch(chr)
		{
		case 'l': return TerrainType.Grass;
		case 'w': return TerrainType.Water;
		case 'r': return TerrainType.Sand;
		case 't': return TerrainType.Tree;
		default: throw new ArgumentException("Illegal terrain type");
		}
	}

	private static char SerializeTerrainTile(TerrainType tile)
	{
		switch(tile)
		{
		case TerrainType.Grass: return 'l';
		case TerrainType.Water: return 'w';
		case TerrainType.Sand: return 'r';
		case TerrainType.Tree: return 't';
		default: return ' ';
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

	private static YamlNode SerializeTerrain(GameObject terrain)
	{
		var tiles = terrain.GetComponentsInChildren<TerrainTile>().ToDictionary(x => x.gameObject.Coordinate(), x => x.type);
		var xMax = tiles.Max(t => t.Key.x);
		var zMax = tiles.Max(t => t.Key.z);
		return new YamlScalarNode(string.Join("\n", Enumerable.Range(0, xMax+1).Select(i =>
		{
			return string.Join("", Enumerable.Range(0, zMax+1).Select(j => 
					{
						var coord = new Coordinate(i, j);
						if (tiles.ContainsKey(coord))
							return SerializeTerrainTile(tiles[new Coordinate(i, j)]).ToString();
						else
							return "";
					}).ToArray());
			}).ToArray()) + "\n") {
			Style = YamlDotNet.Core.ScalarStyle.Literal
		};
	}

	private static ResourceCollection DeserializeResourceCollection(YamlNode node)
	{
		var dict = node.ToDictionary<ResourceType, int>(YamlExtensions.ToEnum<ResourceType>, YamlExtensions.ToInt);
		return ResourceCollection.FromMultiset(dict);
	}

	private static YamlNode SerializeResourceCollection(ResourceCollection collection)
	{
		return new YamlMappingNode(collection.ToMultiset().Select(x =>
			new KeyValuePair<YamlNode, YamlNode>(SerializeScalar(x.Key), SerializeScalar(x.Value))));
	}

	private static YamlNode SerializeScalar<T>(T item)
	{
		return new YamlScalarNode(item.ToString());
	}

	// Deserialize a level yaml string
	public static LevelManager DeserializeLevel(String levelName)
	{
		Debug.Log("Deserializing " + levelName);
		// Factor out to ResourcesPathfinder
		var levelFile = UnityEngine.Resources.Load<TextAsset>("Levels/" + levelName);
		var input = new StringReader(levelFile.text);
		var yaml = new YamlStream();
		yaml.Load(input);

		var levelMapping = yaml.Documents[0].RootNode.AsMapping();

		var level = new GameObject("Level").AddComponent<LevelManager>();
		level.levelName = levelName;
		if (levelMapping.HasKey("Instructions"))
		{
			level.instructions = levelMapping.GetString("Instructions");
		}
		else
		{
			level.instructions = "";
		}

		var terrain = level.terrainController = level.gameObject.AddChildWithComponent<TerrainController>("Terrain");
		foreach (var entry in DeserializeTerrain(levelMapping.GetString("Terrain")))
		{
			terrain.AddTerrainTile(entry.Key, entry.Value);
		}

		var creatures = level.creatureController = level.gameObject.AddChildWithComponent<CreatureController>("Creatures");
		var creatureMapping = DeserializeCoordinateMap(levelMapping.GetMapping("Creatures"), x => DeserializeCreatureType(x));
		foreach (var entry in creatureMapping)
		{
			creatures.AddCreature(entry.Key, entry.Value);
		}

		var resources = level.resourceController = level.gameObject.AddChildWithComponent<ResourceController>("Resources");
		var resourcesMapping = DeserializeCoordinateMap(levelMapping.GetMapping("Resources"), x => DeserializeResourceCollection(x));
		foreach (var entry in resourcesMapping)
		{
			resources.AddResourcePile(entry.Key, entry.Value);
		}

		var recipes = level.recipeController = level.gameObject.AddChildWithComponent<RecipeController>("Recipes");
		var recipeMapping = levelMapping.GetMapping("Recipes");

		var available = recipeMapping.GetSequence("Available").Select(x => DeserializeCreatureType(x)).ToArray();
		recipes.availableRecipes = available;

		var field = DeserializeCoordinateMap(recipeMapping.GetMapping("Field"), x => DeserializeCreatureType(x));
		foreach (var entry in field)
		{
			recipes.AddRecipe(entry.Key, entry.Value);
		}
	
		var goals = level.goalController = level.gameObject.AddChildWithComponent<GoalController>("Goals");
		var goalMapping = DeserializeCoordinateMap(levelMapping.GetMapping("Goals"), x => DeserializeCreatureType(x));
		foreach (var goal in goalMapping)
		{
			goals.AddGoal(goal.Key, goal.Value);
		}

		Debug.Log("Finished deserializing " + levelName);

		return level;
	}

	// Serialize the level into the given writer and return the serialized level
	public static string SerializeLevel(TextWriter writer)
	{
		var terrain = GameObject.Find("Terrain").GetComponent<TerrainController>();
		var terrainString = SerializeTerrain(terrain.gameObject);

		var creatures = GameObject.Find("Creatures").GetComponent<CreatureController>();
		var creatureMapping = SerializeCoordinateMap<Creature>(creatures.gameObject, x => SerializeScalar(x.creatureType));

		var resources = GameObject.Find("Resources").GetComponent<ResourceController>();
		var resourceMapping = SerializeCoordinateMap<ResourcePile>(resources.gameObject, x => SerializeResourceCollection(x.ResourceCollection));

		var recipes = GameObject.Find("Recipes").GetComponent<RecipeController>();
		var availableRecipesList = new YamlSequenceNode(recipes.availableRecipes.Select(x => SerializeScalar(x)));
		var fieldRecipesMapping = SerializeCoordinateMap<Recipe>(recipes.gameObject, x => SerializeScalar(x.creature));
		var recipeNode = new YamlMappingNode()
		{
			{SerializeScalar("Available"), availableRecipesList},
			{SerializeScalar("Field"), fieldRecipesMapping}
		};

		var goals = GameObject.Find("Goals").GetComponent<GoalController>();
		var goalsMapping = SerializeCoordinateMap<Goal>(goals.gameObject, x => SerializeScalar(x.winningCreatureType));

		var levelNode = new YamlMappingNode()
		{
			{"Terrain", terrainString},
			{"Creatures", creatureMapping},
			{"Resources", resourceMapping},
			{"Recipes", recipeNode},
			{"Goals", goalsMapping}
		};

		var level = GameObject.Find("Level").GetComponent<LevelManager>();
		if (!string.IsNullOrEmpty(level.instructions))
		{
			levelNode.Add("Instructions", new YamlScalarNode(level.instructions) { Style = YamlDotNet.Core.ScalarStyle.Literal } );
		}
		var levelDocument = new YamlDocument(levelNode);

		// Serialize
		var yaml = new YamlStream(levelDocument);
		yaml.Save(writer, false);
		return writer.ToString();
	}

	public static IList<String> DeserializeLevelList()
	{
		var levelListFile = UnityEngine.Resources.Load<TextAsset>("level-list");
		var input = new StringReader(levelListFile.text);
		var yaml = new YamlStream();
		yaml.Load(input);
		return yaml.Documents[0].RootNode.AsSequence().Select(x => x.ToString()).ToList();
	}

	public static IAbilityDefinition DeserializeAbility(YamlNode node)
	{
		var ability = node.AsMapping();
		switch(ability.GetString("Type"))
		{
		case "CarryResourceAbility":
			return new CarryResourceAbility.Definition
			{
				Capacity = ability.GetInt("Capacity")
			};
		case "ChangeTerrainAbility":
			return new ChangeTerrainAbility.Definition
			{
				CarryType = ability.GetChild("CarryType").ToEnum<TerrainType>(),
				LeaveType = ability.GetChild("LeaveType").ToEnum<TerrainType>()
			};
		case "FightAbility":
			return new FightAbility.Definition
			{
				Attack = ability.GetChild("Attack").ToEnum<BattlePower>(),
				Defense = ability.GetChild("Defense").ToEnum<BattlePower>()
			};
		case "HealAbility":
			return new HealAbility.Definition
			{
				HealPower = ability.GetInt("HealPower")
			};
		default: throw new ArgumentException("Ability not found: " + ability.GetString("Type"));
		}
	}

	// Creature definitions
	public static CreatureDefinition DeserializeCreature(YamlNode node)
	{
		var definition = node.AsMapping();
		var recipe = DeserializeResourceCollection(definition.GetMapping("Recipe")).ToMultiset();
		var noEnergy = definition.HasKey("NoEnergy") ? definition.GetBool("NoEnergy") : false;
		var allowedTerrain = definition.GetSequence("AllowedTerrain").Select(x => x.ToEnum<TerrainType>()).ToArray();
		var speed = definition.GetChild("Speed").ToEnum<CreatureSpeed>();
		var ability = definition.HasKey("Ability") ? DeserializeAbility(definition.GetChild("Ability")) : null;
		var isEnemy = definition.HasKey("IsEnemy") ? definition.GetBool("IsEnemy") : false;
		return new CreatureDefinition()
		{
			Recipe = recipe,
			NoEnergy = noEnergy,
			AllowedTerrain = allowedTerrain,
			Speed = speed,
			Ability = ability,
			IsEnemy = isEnemy
		};
	}

	public static IDictionary<CreatureType, CreatureDefinition> DeserializeCreatureDefinitions()
	{
		var creaturesFile = UnityEngine.Resources.Load<TextAsset>("creatures");
		var input = new StringReader(creaturesFile.text);
		var yaml = new YamlStream();
		yaml.Load(input);
		var creatureMapping = yaml.Documents[0].RootNode.AsMapping();
		// TODO replace the enum definition
		return creatureMapping.ToDictionary(x => DeserializeCreatureType(x.Key), x => DeserializeCreature(x.Value));
	}
}
