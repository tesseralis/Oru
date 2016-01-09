using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Util;

/// <summary>
/// Manages the overall state of the terrain in the game,
/// and the interaction between the terrain blocks and other
/// game elements.
/// </summary>
public class TerrainController : MonoBehaviour
{
	public TerrainPrefabOptions terrainPrefabs;

	private IDictionary<Coordinate, TerrainTile> grid;

	public TerrainType this[Coordinate coordinate]
	{
		get
		{
			return grid[coordinate].type;
		}
		set
		{
			if (grid.ContainsKey(coordinate))
			{
				// Delete the old value
				Destroy(grid[coordinate].gameObject);
			}

			// Create the new item
			var prefab = terrainPrefabs.PrefabFor (value);
			var block = gameObject.AddChildWithComponent<TerrainTile>(prefab, coordinate);
			block.type = value;
			grid[coordinate] = block;

		}
	}

	public bool Contains(Coordinate coordinate)
	{
		return grid.ContainsKey(coordinate);
	}

	// Initialize the grid
	void Start ()
	{
		grid = new Dictionary<Coordinate, TerrainTile>();
		foreach (var block in GetComponentsInChildren<TerrainTile>())
		{
			grid[block.gameObject.Coordinate()] = block;
		}
	}

}

[Serializable]
public class TerrainPrefabOptions
{
	public GameObject landPrefab;
	public GameObject rockPrefab;
	public GameObject treePrefab;
	public GameObject waterPrefab;


	public GameObject PrefabFor (TerrainType type)
	{
		switch (type) {
		case TerrainType.Land:
			return landPrefab;
		case TerrainType.Rock:
			return rockPrefab;
		case TerrainType.Tree:
			return treePrefab;
		case TerrainType.Water:
			return waterPrefab;
		default:
			throw new ArgumentException("Illegal terrain type: " + type, "type");
		}
	}
}