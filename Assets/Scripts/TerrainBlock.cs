using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util;

/// <summary>
/// Behavior for a single terrain block (a space that can hold a creature).
/// </summary>
public class TerrainBlock: MonoBehaviour
{

	public TerrainType type;

	void OnMouseOver ()
	{
		// TODO make this a delegate somehow?
		GameManager.gm.recipes.OnMouseOverTerrain(this.Coordinate());
	}
	
	void OnMouseDown ()
	{
		GameManager.gm.creatures.SetSelectedCreatureGoal(this.Coordinate());
		GameManager.gm.recipes.OnMouseDownTerrain(this.Coordinate());
	}

}