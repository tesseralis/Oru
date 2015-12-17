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

	public Action<Coordinate> MouseEnterBlock;
	public Action<Coordinate> MouseExitBlock;
	public Action<Coordinate> ClickBlock;

	private IDictionary<Coordinate, TerrainBlock> grid;

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

			// Create the new item
			var prefab = PrefabFor (value);
			var block = this.AddChildWithComponent<TerrainBlock>(prefab, coordinate);
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
		grid = new Dictionary<Coordinate, TerrainBlock>();
		foreach (var block in GetComponentsInChildren<TerrainBlock>())
		{
			grid[block.Coordinate()] = block;
		}
	}

	GameObject PrefabFor (TerrainType type)
	{
		switch (type) {
		case TerrainType.Grass:
			return terrainPrefabs.grassPrefab;
		case TerrainType.Rock:
			return terrainPrefabs.rockPrefab;
		case TerrainType.Tree:
			return terrainPrefabs.treePrefab;
		case TerrainType.Water:
			return terrainPrefabs.waterPrefab;
		default:
			throw new ArgumentException("Illegal terrain type", type.ToString());
		}
	}

}

[Serializable]
public class TerrainPrefabOptions
{
	// TODO make this a dropdown class
	public GameObject grassPrefab;
	public GameObject rockPrefab;
	public GameObject treePrefab;
	public GameObject waterPrefab;
}