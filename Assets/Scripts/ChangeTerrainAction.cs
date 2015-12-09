using UnityEngine;
using System.Collections;

public class ChangeTerrainAction : MonoBehaviour, IAction
{
	public void Act(Coordinate target)
	{
		// TODO change, not destroy, the target.
		Debug.LogFormat("Acting on coordinate {0}.", target);
		Destroy(GameManager.gm.terrain.TerrainGrid[target].gameObject);
		GameManager.gm.terrain.TerrainGrid.Remove(target);
	}
}
