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

	private float cellSize = 2;

	// Initialize the grid
	void Start ()
	{
		grid = new Dictionary<Coordinate, TerrainType>();
		// Assume this grid's children are all terrain blocks
		foreach (Transform terrainBlock in gameObject.transform)
		{
			Coordinate blockCoordinate = CalculateGridCoordinate(terrainBlock.position);
			grid[blockCoordinate] = terrainBlock.gameObject.GetComponent<TerrainBlock>().type;
		}
	}

	// TODO figure out the right place to abstract this.
	private Coordinate CalculateGridCoordinate(Vector3 position)
	{
		return new Coordinate((int)(position.x / cellSize), (int)(position.z / cellSize));
	}
}
