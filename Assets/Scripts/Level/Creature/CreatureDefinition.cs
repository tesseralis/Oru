using System;
using System.Collections.Generic;
using Util;

/// <summary>
/// Defines the static variables defining a creature
/// </summary>
public class CreatureDefinition
{
	public string Description { get; set; }
	public IDictionary<ResourceType, int> Recipe { get; set; }
	public TerrainType[] AllowedTerrain { get; set; }
	public CreatureSpeed Speed { get; set; }
	public IAbilityDefinition Ability { get; set; }
	public bool IsEnemy { get; set; }

	public bool NeedsEnergy()
	{
		return Recipe.Contains(ResourceType.Energy);
	}
}
