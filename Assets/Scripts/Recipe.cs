using UnityEngine;
using System.Collections;

public class Recipe : MonoBehaviour
{

	// The type of creature that this recipe creates;
	public CreatureType creature;

	public Coordinate Coordinate
	{
		get
		{
			// TODO do a lazy eval to save instantiation cost?
			return GameManager.gm.ToGridCoordinate(this.transform.position);
		}
	}
}
