using UnityEngine;
using System.Collections;

public class ActionMarker : MonoBehaviour
{

	void OnMouseDown()
	{
		Vector3 position = gameObject.transform.position;
		var action = GetComponentInParent<ActionMarkers>().Action;
		if (action != null)
		{
			action.Act(CalculateGridCoordinate(position));
		}
	}

	private float cellSize = 2;
	// TODO figure out the right place to abstract this.
	private Coordinate CalculateGridCoordinate(Vector3 position)
	{
		return new Coordinate((int)(position.x / cellSize), (int)(position.z / cellSize));
	}
}
