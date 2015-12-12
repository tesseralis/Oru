using System.Collections.Generic;

/// <summary>
/// Defines the static variables defining a creature
/// </summary>
public class CreatureDefinition
{
	public CreatureType Type { get; set; }
	public IDictionary<ResourceType, int> Recipe { get; set; }
	public TerrainType[] AllowedTerrain { get; set; }	
}

// TODO move Creatures to a separate class?
public static class Creatures
{
	public static readonly CreatureDefinition Duck = new CreatureDefinition
	{
		Type = CreatureType.Duck,
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Yellow, 1} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Grass, TerrainType.Water }
	};

	public static readonly CreatureDefinition Dragon = new CreatureDefinition
	{
		Type = CreatureType.Dragon,
		Recipe = new Dictionary<ResourceType, int>() { {ResourceType.Red, 5} },
		AllowedTerrain = new TerrainType[]{ TerrainType.Grass, TerrainType.Rock }
	};

	public static CreatureDefinition ForType(CreatureType type)
	{
		switch(type)
		{
		case CreatureType.Duck:
			return Duck;
		case CreatureType.Dragon:
			return Dragon;
		default:
			throw new System.ArgumentException("Passed in an invalid creature type", type.ToString());
		}
	}
}
