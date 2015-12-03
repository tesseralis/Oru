using UnityEngine;
using System.Collections;

public class Creature : MonoBehaviour
{

	public Cell.cellTypes[] allowedCells = {Cell.cellTypes.Grass};

	void OnMouseDown()
	{
		GameManager.gm.SetCurrentCreature (gameObject);
	}
}
