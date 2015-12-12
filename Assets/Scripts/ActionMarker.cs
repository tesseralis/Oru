using Util;
using UnityEngine;
using System;
using System.Collections;

public class ActionMarker : MonoBehaviour
{

	public Action<Coordinate> OnClick;

	void OnMouseDown()
	{
		if (OnClick != null)
		{
			OnClick(this.Coordinate());
			// Once we're done, deactivate ourselves
			// TODO make this another delegate, somehow
			GameManager.gm.creatures.IsActing = false;
		}
	}

}
