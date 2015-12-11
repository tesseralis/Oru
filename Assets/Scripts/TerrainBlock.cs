using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Behavior for a single terrain block (a space that can hold a creature).
/// </summary>
public class TerrainBlock: MonoBehaviour
{

	public TerrainType type;

	public Coordinate Coordinate
	{
		get
		{
			// TODO do a lazy eval to save instantiation cost?
			return GameManager.gm.ToGridCoordinate(this.transform.position);
		}
	}

	void OnMouseOver ()
	{
		// TODO make this a delegate somehow?
		GameManager.gm.recipes.OnMouseOverTerrain(Coordinate);
	}
	
	void OnMouseDown ()
	{
		GameManager.gm.creatures.SetSelectedCreatureGoal(Coordinate);
		GameManager.gm.recipes.OnMouseDownTerrain(Coordinate);
	}

}