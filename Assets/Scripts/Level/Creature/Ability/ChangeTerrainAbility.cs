using UnityEngine;
using System.Collections;
using System.Linq;
using Util;

public class ChangeTerrainAbility : MonoBehaviour, IAbility
{
	public TerrainType carryType = TerrainType.Rock;
	public TerrainType leaveType = TerrainType.Land;

	public bool IsCarrying { get; private set; }

	public void Awake()
	{
		IsCarrying = false;
	}

	public void Use(Coordinate target)
	{
		TerrainController terrain = LevelManager.Terrain;

		// Determine whether the creature should be picking up or putting down
		TerrainType initialType;
		TerrainType finalType;
		if (IsCarrying)
		{
			initialType = leaveType;
			finalType = carryType;
		} else {
			initialType = carryType;
			finalType = leaveType;
		}

		// Pick up or put down the terrain.
		IsCarrying = !IsCarrying;
		if (terrain.Contains(target) && terrain[target] == initialType
			&& LevelManager.Resources[target].IsEmpty()
			&& CanSetCoordinate(target, finalType))
		{
			terrain[target] = finalType;
		}
	}

	// Returns true if you can set the coordinate to the specified terrain type
	private bool CanSetCoordinate(Coordinate target, TerrainType terrain)
	{
		return !LevelManager.Creatures.CreatureList.Any(x => (x.Position == target || x.NextPosition == target)
			&& !x.Definition.AllowedTerrain.Contains(terrain));
	}

}
