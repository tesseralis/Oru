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
		if (controller.OnHover != null) { controller.OnHover(this.Coordinate()); }
	}
	
	void OnMouseDown ()
	{
		if (controller.OnClick != null) { controller.OnClick(this.Coordinate()); }
	}

}
