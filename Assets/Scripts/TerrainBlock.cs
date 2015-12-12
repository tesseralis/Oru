using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Util;

/// <summary>
/// Behavior for a single terrain block (a space that can hold a creature).
/// </summary>
public class TerrainBlock: MonoBehaviour
{
	private TerrainController controller {
		get { return GetComponentInParent<TerrainController>(); }
	}

	public TerrainType type;

	void OnMouseOver ()
	{
		controller.OnHover(this.Coordinate());
	}
	
	void OnMouseDown ()
	{
		controller.OnClick(this.Coordinate());
	}

}