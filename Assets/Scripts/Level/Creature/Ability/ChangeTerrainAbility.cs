using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;

public class ChangeTerrainAbility : AbstractCarryAbility, IAbility
{
	public TerrainType carryType = TerrainType.Rock;
	public TerrainType leaveType = TerrainType.Land;

	public class Definition : IAbilityDefinition
	{
		public TerrainType CarryType { get; set; }
		public TerrainType LeaveType { get; set; }

		public string Description()
		{
			return "Pick up " + CarryType;
		}

		public IAbility AddToCreature(Creature creature)
		{
			var ability = creature.gameObject.AddComponent<ChangeTerrainAbility>();
			ability.carryType = CarryType;
			ability.leaveType = LeaveType;
			return ability;
		}
	}

	public bool isCarrying;

	protected override void DoPickup(Coordinate target)
	{
		TerrainController terrain = LevelManager.Terrain;

		// Determine whether the creature should be picking up or putting down
		TerrainType initialType;
		TerrainType finalType;
		if (isCarrying)
		{
			initialType = leaveType;
			finalType = carryType;
		} else {
			initialType = carryType;
			finalType = leaveType;
		}

		// Pick up or put down the terrain if we can
		if (terrain.Contains(target) && terrain[target] == initialType
			&& LevelManager.Resources[target].IsEmpty()
			&& CanSetCoordinate(target, finalType))
		{
			isCarrying = !isCarrying;
			terrain[target] = finalType;
		}
	}

	public override bool CanUse(Coordinate coordinate)
	{
		return LevelManager.Terrain[coordinate] == (isCarrying ? leaveType : carryType);
	}

	// Returns true if you can set the coordinate to the specified terrain type
	private bool CanSetCoordinate(Coordinate target, TerrainType terrain)
	{
		return !LevelManager.Creatures.CreatureList.Any(x => (x.Position == target || x.NextPosition == target)
			&& !x.Definition.AllowedTerrain.Contains(terrain));
	}

	public override string Description()
	{
		if (isCarrying)
		{
			return "Put down " + carryType;
		}
		else
		{
			return "Pick up " + carryType;
		}
	}
}
