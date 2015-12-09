using UnityEngine;
using System.Collections;

public class ChangeTerrainAction : MonoBehaviour, IAction
{
	public void Act(Coordinate target)
	{
		// TODO carry the target and be able to put it down.
		Debug.LogFormat("Acting on coordinate {0}.", target);
		GameManager.gm.terrain[target] = TerrainType.Grass;
	}
}
