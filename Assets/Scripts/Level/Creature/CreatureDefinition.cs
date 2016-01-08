using System;
using System.Collections.Generic;

/// <summary>
/// Defines the static variables defining a creature
/// </summary>
public class CreatureDefinition
{
	public CreatureType Type { get; set; }
	public string Description { get; set; }
	public IDictionary<ResourceType, int> Recipe { get; set; }
	public TerrainType[] AllowedTerrain { get; set; }
	public Func<Creature, CreatureSpeed> Speed { get; set; }
	public IAbilityDefinition Ability { get; set; }
	public bool IsEnemy { get; set; }
}
