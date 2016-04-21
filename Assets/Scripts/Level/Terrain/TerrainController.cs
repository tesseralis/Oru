using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;

/// <summary>
/// Manages the overall state of the terrain in the game,
/// and the interaction between the terrain blocks and other
/// game elements.
/// </summary>
public class TerrainController : MonoBehaviour
{
	private IDictionary<Coordinate, TerrainTile> grid = new Dictionary<Coordinate, TerrainTile>();

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
			grid[coordinate] = AddTerrainTile(coordinate, value);
		}
	}

	public int Width
	{
		get { return grid.Keys.Max(c => c.x) - grid.Keys.Min(c => c.x) + 1; }
	}

	public int Height
	{
		get { return grid.Keys.Max(c => c.z) - grid.Keys.Min(c => c.z) + 1; }
	}

	// Add a terrain tile to this object, but not the game state
	public TerrainTile AddTerrainTile(Coordinate coordinate, TerrainType type)
	{
		var prefab = ResourcesPathfinder.TerrainPrefab(type);
		var tile = gameObject.AddChildWithComponent<TerrainTile>(prefab, coordinate);
		tile.type = type;
		return tile;
	}

	public ICollection<Coordinate> GetCoordinates()
	{
		return grid.Keys;
	}

	public bool Contains(Coordinate coordinate)
	{
		return grid.ContainsKey(coordinate);
	}

	// Initialize the grid
	void Start ()
	{
		foreach (var block in GetComponentsInChildren<TerrainTile>())
		{
			grid[block.gameObject.Coordinate()] = block;
		}
	}

}
