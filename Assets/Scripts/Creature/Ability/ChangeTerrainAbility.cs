using UnityEngine;
using System.Collections;

public class ChangeTerrainAbility : MonoBehaviour, IAbility
{
	public TerrainType carryType = TerrainType.Rock;
	public TerrainType leaveType = TerrainType.Land;

	private bool isCarrying = false;

	public void Use(Coordinate target)
	{
		TerrainController terrain = GameManager.Terrain;

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

		// Pick up or put down the terrain.
		// FIXME make sure that the new location can support the creature that is there now
		isCarrying = !isCarrying;
		if (terrain.Contains(target) && terrain[target] == initialType)
		{
			terrain[target] = finalType;
		}
	}
}
