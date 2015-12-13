using Util;
using UnityEngine;
using System;
using System.Collections;

public class ActionMarker : MonoBehaviour
{

	public Action<Coordinate> OnClick;

	// TODO Maybe we can consolidate most of these "on click" objects?
	void OnMouseDown()
	{
		if (OnClick != null) { OnClick(this.Coordinate()); }
	}

}
