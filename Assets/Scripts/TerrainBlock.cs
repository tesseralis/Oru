using UnityEngine;
using System.Collections;

/// <summary>
/// Behavior for a single terrain block (a space that can hold a creature).
/// </summary>
public class TerrainBlock: MonoBehaviour
{

	public TerrainType type;

	public Coordinate Coordinate
	{
		get
		{
			// TODO do a lazy eval to save instantiation cost?
			return CalculateGridCoordinate(this.transform.position);
		}
	}
	
	void OnMouseDown ()
	{
		// Set the creature's destination to this block.
		Debug.Log("Setting goal of " + GameManager.gm.creatures.SelectedCreature);
		GameManager.gm.creatures.SelectedCreature.Goal = Coordinate;
	}
	
	private float cellSize = 2;

	// TODO figure out the right place to abstract this.
	private Coordinate CalculateGridCoordinate(Vector3 position)
	{
		return new Coordinate((int)(position.x / cellSize), (int)(position.z / cellSize));
	}
}