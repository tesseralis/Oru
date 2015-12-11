using UnityEngine;
using System.Collections;

public class Instruction : MonoBehaviour
{

	// The type of creature that this instruction creates;
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
