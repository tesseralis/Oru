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
			return GameManager.gm.ToGridCoordinate(this.transform.position);
		}
	}

	void OnMouseOver ()
	{
		GameObject createMarker = GameManager.gm.createMarker;
		var next = Coordinate;
		if (createMarker)
		{
			var cellSize = GameManager.gm.cellSize;
			// Update the position visually
			// TODO factor out this method into the GameManager
			createMarker.transform.position = new Vector3(next.x * cellSize, gameObject.transform.position.y, next.z * cellSize);

		}
	}
	
	void OnMouseDown ()
	{
		// Set the creature's destination to this block.
		var creature = GameManager.gm.creatures.SelectedCreature;
		if (creature)
		{
			Debug.LogFormat("Setting goal of {0} to {1}", creature, Coordinate);
			creature.Goal = Coordinate;
		}
	}

}