using System;
using System.Collections.Generic;

/// <summary>
/// Statically defines the creatures available in this game.
/// </summary>
public static class Creatures
{
	public static readonly CreatureDefinition Crane = new CreatureDefinition
	{
		Type = CreatureType.Crane,
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 4} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Water }
	};
	
	public static readonly CreatureDefinition Turtle = new CreatureDefinition
	{
		Type = CreatureType.Turtle,
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Green, 9} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Water }
	};

	public static readonly CreatureDefinition Horse = new CreatureDefinition
	{
		Type = CreatureType.Horse,
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Red, 9} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Land, TerrainType.Rock }
	};

	public static readonly CreatureDefinition Elephant = new CreatureDefinition
	{
		Type = CreatureType.Elephant,
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Energy, 1}, {ResourceType.Blue, 25} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Land }
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
