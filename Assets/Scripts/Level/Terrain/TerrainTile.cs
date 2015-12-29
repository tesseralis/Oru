using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util;

/// <summary>
/// Behavior for a single terrain block (a space that can hold a creature).
/// </summary>
public class TerrainTile: MonoBehaviour
{
	private TerrainController controller {
		get { return GetComponentInParent<TerrainController>(); }
	}

	public TerrainType type;

	void OnMouseEnter ()
	{
		if (controller.MouseEnterBlock != null) { controller.MouseEnterBlock(gameObject.Coordinate()); }
	}

	void OnMouseExit ()
	{
		if (controller.MouseExitBlock != null) { controller.MouseExitBlock(gameObject.Coordinate()); }
	}

}
