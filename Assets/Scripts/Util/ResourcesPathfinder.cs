using UnityEngine;
using System;

/// <summary>
/// A pathfinder for resource assets
/// </summary>
public static class ResourcesPathfinder
{
	private const int lowThreshold = CreatureController.lowHealth;
	private const string prefabPath = "Prefabs";

	private static GameObject LoadPrefab(params string[] path)
	{
		var prefab = Resources.Load<GameObject>(string.Format("{0}/{1}", prefabPath, string.Join("/", path)));
		if (!prefab)
		{
			throw new ArgumentException("Bad path when loading prefab at: " + string.Join("/", path));
		}
		return prefab;
	}

	public static GameObject TerrainPrefab(TerrainType type)
	{
		return LoadPrefab("Terrain", type.ToString());
	}

	public static GameObject CreaturePrefab(CreatureType type)
	{
		return LoadPrefab("Creatures", type.ToString());
	}

	public static GameObject ResourcePilePrefab()
	{
		return LoadPrefab("Resources", "ResourcePile");
	}

	public static GameObject PaperResourcePrefab(ResourceType type)
	{
		return LoadPrefab("Resources", "Paper", type + "Paper");
	}

	public static GameObject EnergyResourcePrefab(int energy)
	{
		string stem;
		if (energy <= 0)
		{
			stem = "No";
		}
		else if (energy < lowThreshold)
		{
			stem = "Low";
		}
		else if (energy < CreatureController.maxHealth)
		{
			stem = "Medium";
		}
		else
		{
			stem = "Full";
		}
		return LoadPrefab("Resources", "Energy", stem + "Energy");
	}

	public static GameObject RecipePrefab()
	{
		return LoadPrefab("Recipe");
	}

	public static GameObject GoalPrefab()
	{
		return LoadPrefab("Goal");
	}

	public static Sprite GoalSprite(CreatureType? type)
	{
		Sprite[] sprites = Resources.LoadAll<Sprite>(prefabPath + "/Goals");
		return sprites[GetGoalSpriteIndex(type)];
	}

	private static int GetGoalSpriteIndex(CreatureType? type)
	{
		if (type.HasValue)
		{
			return Array.IndexOf(creatureOrder, type.Value.name);
		}
		else
		{
			return 14;
		}
	}

	private static string[] creatureOrder = {
		"Rabbit", "Dog", "Crane", "Bird",
		"Mouse", "Koi", "Horse", "Turtle",
		"Frog", "Snake", "Shark", "Tiger",
		"Butterfly", "Monkey", "Any", "Dragon" };
}
