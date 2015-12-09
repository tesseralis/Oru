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

	public GameObject grassPrefab;
	public GameObject rockPrefab;

	public TerrainType this[Coordinate coordinate]
	{
		get
		{
			return grid[coordinate].type;
		}
		set
		{
			// Delete the old value
			Destroy(grid[coordinate].gameObject);

			// Set the new value
			GameObject prefab;
			switch(value)
			{
			case TerrainType.Grass:
			default:
				prefab = grassPrefab;
				break;
			case TerrainType.Rock:
				prefab = rockPrefab;
				break;
			}
			var cellSize = GameManager.gm.cellSize;
			var position = new Vector3(coordinate.x * cellSize, gameObject.transform.position.y, coordinate.z * cellSize);

			GameObject terrainObject = (GameObject)Instantiate(prefab, position, Quaternion.identity);
			terrainObject.transform.parent = gameObject.transform;

			if (!terrainObject.GetComponent<TerrainBlock>())
			{
				var block = terrainObject.AddComponent<TerrainBlock>();
				block.type = value;
			}

			grid[coordinate] = terrainObject.GetComponent<TerrainBlock>();

		}
	}

	public bool Contains(Coordinate coordinate)
	{
		return grid.ContainsKey(coordinate);
	}

	private IDictionary<Coordinate, TerrainBlock> grid;

	// Initialize the grid
	void Start ()
	{
		// TODO this might fail if any other "start" function requires the grid
		grid = new Dictionary<Coordinate, TerrainBlock>();
		// Assume this grid's children are all terrain blocks
		foreach (Transform child in gameObject.transform)
		{
			TerrainBlock block = child.gameObject.GetComponent<TerrainBlock>();
			grid[block.Coordinate] = block;
		}
	}

}
