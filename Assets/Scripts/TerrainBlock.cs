using UnityEngine;
using System.Collections;

/// <summary>
/// Behavior for a single terrain block (a space that can hold a creature).
/// </summary>
public class TerrainBlock: MonoBehaviour
{
	
	public TerrainType type;
	
	void OnMouseDown ()
	{
		// Set the creature's destination to this block.
		GameManager.gm.SetCurrentCreatureDestination (gameObject);
	}
}