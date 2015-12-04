using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour
{

	public TerrainType[] allowedTerrain = {TerrainType.Grass};

	public Coordinate Goal { get; set; }

	public Coordinate Position
	{
		get { return position; }
	}

	private CreatureManager manager;

	private Coordinate position;

	void Start()
	{
		// Store our initial position
		position = CalculateGridCoordinate(gameObject.transform.position);
		// Assume our parent is our manager
		manager = GetComponentInParent<CreatureManager>();
	}

	void OnMouseDown()
	{
		manager.SelectedCreature = this;
	}

	public void Step()
	{
		if (Goal != null && (!position.Equals(Goal)))
		{
			var next = NextCoordinate ();
			position = next;

			// Update the position visually
			gameObject.transform.position = new Vector3(next.x * cellSize, gameObject.transform.position.y, next.z * cellSize);
		}
	}

	private float cellSize = 2;
	
	// TODO figure out the right place to abstract this.
	private Coordinate CalculateGridCoordinate(Vector3 position)
	{
		return new Coordinate((int)(position.x / cellSize), (int)(position.z / cellSize));
	}

	// do a BFS and figure out the right path
	private Coordinate NextCoordinate()
	{
		// TODO do A* search
		var grid = GameManager.gm.terrain.TerrainGrid;
		var distance = new Dictionary<Coordinate, int>();
		var parents = new Dictionary<Coordinate, Coordinate> ();
		var queue = new Queue<Coordinate> ();
		distance [position] = 0;
		queue.Enqueue (position);
		
		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			// TODO factor out to a separate script
			Coordinate[] neighbors = {
				new Coordinate(current.x, current.z + 1),
				new Coordinate(current.x, current.z - 1),
				new Coordinate(current.x + 1, current.z),
				new Coordinate(current.x - 1, current.z)
			};
			foreach (Coordinate neighbor in neighbors)
			{
				// TODO break when we reach the goal
				if (grid.ContainsKey(neighbor) && allowedTerrain.Contains(grid[neighbor]) && !distance.ContainsKey(neighbor))
				{
					distance[neighbor] = distance[current] + 1;
					parents[neighbor] = current;
					queue.Enqueue(neighbor);
				}
			}
			
		}
		var next = Goal;
		// if we can't get to the destination, return the same item
		if (!parents.ContainsKey(next))
		{
			Debug.Log("Can't reach the intended destination.");
			return position;
		}
		while (!parents[next].Equals (position))
		{
			next = parents[next];
		}
		return next;
	}

}
