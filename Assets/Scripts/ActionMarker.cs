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
			action.Act(GameManager.gm.ToGridCoordinate(position));
		}
	}

}
