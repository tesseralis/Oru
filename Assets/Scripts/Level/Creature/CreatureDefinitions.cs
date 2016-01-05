using System;
using System.Collections.Generic;

/// <summary>
/// Statically defines the creatures available in this game.
/// </summary>
public static class CreatureDefinitions
{
	public static readonly CreatureDefinition Crane = new CreatureDefinition
	{
		Type = CreatureType.Crane,
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Blue, 1} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Water }
	};
	
	public static readonly CreatureDefinition Turtle = new CreatureDefinition
	{
		Type = CreatureType.Turtle,
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Green, 1} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Water },
		Ability = new CarryResourceAbility.Definition { Capacity = 5 }
	};

	public static readonly CreatureDefinition Horse = new CreatureDefinition
	{
		Type = CreatureType.Horse,
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Red, 4} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Rock },
		Ability = new CarryResourceAbility.Definition { Capacity = 5 }
	};

	public static readonly CreatureDefinition Elephant = new CreatureDefinition
	{
		Type = CreatureType.Elephant,
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Blue, 9} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Land },
		Ability = new ChangeTerrainAbility.Definition
		{
			CarryType = TerrainType.Tree,
			LeaveType = TerrainType.Land
		}
	};

	public static CreatureDefinition ForType(CreatureType type)
	{
		switch(type)
		{
		case CreatureType.Crane: return Crane;
		case CreatureType.Turtle: return Turtle;
		case CreatureType.Horse: return Horse;
		case CreatureType.Elephant: return Elephant;
		default: throw new ArgumentException("Passed in an invalid creature type: " + type, "type");
		}
	}

}
