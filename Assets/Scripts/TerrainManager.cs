using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages the overall state of the terrain in the game,
/// and the interaction between the terrain blocks and other
/// game elements.
/// </summary>
public class TerrainManager : MonoBehaviour
{

	/// <summary>
	/// Mapping of grid coordinates to the type of terrain.
	/// </summary>
	/// <value>The terrain grid.</value>
	public IDictionary<Coordinate, TerrainType> TerrainGrid
	{
		get {
			return grid;
		}
	}

	private IDictionary<Coordinate, TerrainType> grid;

	// Initialize the grid
	void Start ()
	{
		// TODO this might fail if any other "start" function requires the grid
		grid = new Dictionary<Coordinate, TerrainType>();
		// Assume this grid's children are all terrain blocks
		foreach (Transform child in gameObject.transform)
		{
			TerrainBlock block = child.gameObject.GetComponent<TerrainBlock>();
			grid[block.Coordinate] = block.type;
		}
	}

}
