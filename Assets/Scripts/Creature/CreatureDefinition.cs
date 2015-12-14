﻿using System.Collections.Generic;

/// <summary>
/// Defines the static variables defining a creature
/// </summary>
public class CreatureDefinition
{
	public CreatureType Type { get; set; }
	public IDictionary<ResourceType, int> Recipe { get; set; }
	public TerrainType[] AllowedTerrain { get; set; }	
}