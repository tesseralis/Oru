using UnityEngine;
using System;
using System.Collections.Generic;

public enum CreatureType
{
	Crane,
	Turtle,
	Horse,
	Bear,
	Crab,
	Wolf,
	Flower,
	Beaver,
	Alligator,
	Serpent,
	Lion,
	Camel,
	Dragon
}

/// <summary>
/// Statically defines the creatures available in this game.
/// </summary>
public static class CreatureDefinitions
{
	// TODO make sure this can be updated in the editor
	private static IDictionary<CreatureType, CreatureDefinition> creatureList = null;

	public static CreatureDefinition ForType(CreatureType type)
	{
		if (creatureList == null)
		{
			creatureList = Serialization.DeserializeCreatureDefinitions();
		}
		return creatureList[type];
	}
}
