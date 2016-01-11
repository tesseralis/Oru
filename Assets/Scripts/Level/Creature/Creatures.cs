using UnityEngine;
using System;
using System.Collections.Generic;

public enum CreatureType
{
	Crane,
	Turtle,
	Horse,
	Elephant,
	Crab,
	Wolf,
	Flower
}

/// <summary>
/// Statically defines the creatures available in this game.
/// </summary>
public static class CreatureDefinitions
{
	private static Func<Creature, CreatureSpeed> FixedSpeed(CreatureSpeed speed)
	{
		return x => speed;
	}

	public static CreatureDefinition ForType(CreatureType type)
	{
		switch(type)
		{
		case CreatureType.Crane:
			return new CreatureDefinition
			{
				Description = "The basic origami crane",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 1} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Water },
				Speed = FixedSpeed(CreatureSpeed.Medium)
			};
		case CreatureType.Turtle:
			return new CreatureDefinition
			{
				Description = "A seabound unit that can carry resources",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Green, 1} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Water },
				Speed = x =>
				{
					switch(LevelManager.Terrain[x.Position])
					{
					case TerrainType.Water: return CreatureSpeed.Medium;
					case TerrainType.Land: return CreatureSpeed.Slow;
					default: throw new InvalidOperationException("Terrain not allowed");
					}
				},
				Ability = new CarryResourceAbility.Definition { Capacity = 5 }
			};
		case CreatureType.Horse:
			return new CreatureDefinition
			{
				Description = "A versatile land unit that can carry resources",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Red, 4} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Rock },
				Speed =  x =>
				{
					if (x.GetComponent<CarryResourceAbility>().Carrying.IsEmpty())
					{
						return CreatureSpeed.Fast;
					}
					else
					{
						return CreatureSpeed.Slow;
					}
				},
				Ability = new CarryResourceAbility.Definition { Capacity = 5 }
			};
		case CreatureType.Elephant:
			return new CreatureDefinition
			{
				Description = "A large unit that can uproot and move trees",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 9} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land },
				Speed = FixedSpeed(CreatureSpeed.Slow),
				Ability = new ChangeTerrainAbility.Definition
				{
					CarryType = TerrainType.Tree,
					LeaveType = TerrainType.Land
				}
			};
		case CreatureType.Crab:
			return new CreatureDefinition
			{
				Description = "A basic enemy creature",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Red, 1} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land },
				Speed = FixedSpeed(CreatureSpeed.Slow),
				Ability = new FightAbility.Definition { Attack = 10, Defense = 5 },
				IsEnemy = true
			};
		case CreatureType.Wolf:
			return new CreatureDefinition
			{
				Description = "Can fight enemies",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 4} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Rock },
				Speed = FixedSpeed(CreatureSpeed.Fast),
				Ability = new FightAbility.Definition { Attack = 10, Defense = 7 }
			};
		case CreatureType.Flower:
			return new CreatureDefinition
			{
				Description = "Can heal other creatures",
				Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Yellow, 1} },
				AllowedTerrain = new TerrainType[]{ TerrainType.Land },
				Speed = FixedSpeed(CreatureSpeed.Idle),
				Ability = new HealAbility.Definition { HealPower = 1 }
			};
		default: throw new ArgumentException("Passed in an invalid creature type: " + type, "type");
		}
	}

}

[Serializable]
public class CreaturePrefabOptions
{
	public GameObject cranePrefab;
	public GameObject turtlePrefab;
	public GameObject horsePrefab;
	public GameObject elephantPrefab;
	public GameObject crabPrefab;
	public GameObject wolfPrefab;
	public GameObject flowerPrefab;

	public GameObject PrefabFor (CreatureType creature)
	{
		switch (creature) {
		case CreatureType.Crane: return cranePrefab;
		case CreatureType.Turtle: return turtlePrefab;
		case CreatureType.Horse: return horsePrefab;
		case CreatureType.Elephant: return elephantPrefab;
		case CreatureType.Crab: return crabPrefab;
		case CreatureType.Wolf: return wolfPrefab;
		case CreatureType.Flower: return flowerPrefab;
		default: throw new ArgumentException("Illegal creature type: " + creature, "creature");
		}
	}
}
