using UnityEngine;
using System.Collections;

public class ActionMarker : MonoBehaviour
{

	public IAction Action { set; get; }


	void OnMouseDown()
	{
		Vector3 position = gameObject.transform.position;
		if (Action != null)
		{
			Action.Act(GameManager.gm.ToGridCoordinate(position));
			// Once we're done, deactivate ourselves
			GameManager.gm.creatures.IsActing = false;
		}
	}

}
