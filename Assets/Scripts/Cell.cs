using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{

	public enum cellTypes { Grass, Rock }

	public cellTypes cellType;
	
	void OnMouseDown ()
	{
		GameManager.gm.SetCurrentCreatureDestination (gameObject);
	}
}