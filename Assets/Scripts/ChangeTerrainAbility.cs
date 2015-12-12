using UnityEngine;
using System.Collections;

public class ChangeTerrainAbility : MonoBehaviour, IAbility
{
	public TerrainType carryType = TerrainType.Rock;
	public TerrainType leaveType = TerrainType.Grass;

	private bool isCarrying = false;

	public void Use(Coordinate target)
	{
		TerrainManager terrain = GameManager.gm.terrain;

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
		isCarrying = !isCarrying;
		if (terrain.Contains(target) && terrain[target] == initialType)
		{
			terrain[target] = finalType;
		}
	}
}
