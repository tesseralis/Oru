using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

/// <summary>
/// A creature represents a single movable unit in the game, whether friendly or not.
/// </summary>
public class Creature : MonoBehaviour
{
	// The type of creature this is.
	public CreatureType creatureType;
	public int health = ResourceCollection.maxHealth;

	public Coordinate Position { get; private set; }
	public Coordinate NextPosition { get; private set; }
	public Coordinate Goal { get; private set; }
	public IAbility Ability { get; private set; }

	private bool isMoving = false;
	// Switch to make sure enemy only moves every two steps
	private bool enemyToggle = false;
	private int prevStep, nextStep;

	private static System.Random rnd = new System.Random();
	
	// Convenience method to get the creature's definition
	public CreatureDefinition Definition
	{
		get { return CreatureDefinitions.ForType(creatureType); }
	}

	public ResourceCollection ToResources()
	{
		if (Definition.NeedsEnergy())
		{
			return ResourceCollection.FromMultiset(Definition.Recipe.MultisetSubtract(ResourceType.Energy, 1)).Add(health);
		}
		else
		{
			return ResourceCollection.FromMultiset(Definition.Recipe);
		}
	}

	void Awake()
	{
		// Make sure we have the right ability attached
		if (Definition.Ability != null)
		{
			Ability = Definition.Ability.AddToCreature(this);
		}
	}

	void Start()
	{
		// Store our initial position
		Goal = NextPosition = Position = gameObject.Coordinate();
		prevStep = LevelManager.level.Steps;
		nextStep = prevStep + 4 - (int)Definition.Speed(this);
	}

	public void Update()
	{
		// Animate the creature moving
		var stepInterval = LevelManager.level.stepInterval;
		var cellSize = LevelManager.level.cellSize;

		var time = Time.timeSinceLevelLoad;
		var ratio = ((time / stepInterval) + 1 - prevStep) / (nextStep - prevStep);
		var direction = NextPosition - Position;
		var translation = new Vector3(direction.x, 0, direction.z) * ratio * cellSize;
		gameObject.SetPosition(Position);
		transform.position += translation;
	}

	public void Step()
	{
		if (LevelManager.level.Steps >= nextStep)
		{
			prevStep = nextStep;
			nextStep += 4 - (int)Definition.Speed(this);

			// If the creature has a passive ability, do it
			if (HasAbility())
			{
				Ability.Passive();
			}

			// Move non-idle creatures
			if (Definition.Speed(this) != CreatureSpeed.Idle)
			{
				if (!Definition.IsEnemy)
				{
					FriendlyStep();
				}
				else
				{
					EnemyStep();
				}
			}

		}
	}

	private void EnemyStep()
	{
		Position = NextPosition;
		enemyToggle = !enemyToggle;
		var possible = Position.CardinalNeighbors().Where(IsValidCoordinate).ToList();
		var nextToEnemies = Position.CardinalNeighbors().Any(x =>
			{
				var creature = LevelManager.Creatures[x];
				return creature != null && !creature.Definition.IsEnemy;
			});
		if (enemyToggle && !nextToEnemies)
		{
			if (possible.Count > 0)
			{
				NextPosition = possible[rnd.Next(possible.Count)];
			}
			Coordinate direction;
			if (NextPosition != Position)
			{
				direction = NextPosition - Position;
			}
			else
			{
				direction = Coordinate.cardinals[rnd.Next(4)];
			}
			FaceDirection (direction);
		}
	}

	private void FriendlyStep()
	{
		Position = NextPosition;
		if (!Position.Equals(Goal) && health > 0)
		{
			NextPosition = NextCoordinate ();

			// Make our creature face the right direction
			if (NextPosition != Position)
			{
				health -= 1;
				var direction = NextPosition - Position;
				FaceDirection (direction);
			}

			// Animate our creature if it has animation
			// TODO factor out animation into a UX class
			if (GetComponentInChildren<Animator>() && !isMoving)
			{
				isMoving = true;
				Debug.Log("Animating the creature moving.");
				GetComponentInChildren<Animator>().SetTrigger("StartMove");
			}
		}
		else
		{
			if (GetComponentInChildren<Animator>() && isMoving)
			{
				isMoving = false;
				Debug.Log("Animating the creature stopping.");
				GetComponentInChildren<Animator>().SetTrigger("StopMove");
			}
		}


		// If the creature loses all health, set it to an idle state
		if (health == 0)
		{
			Goal = Position;
		}
	}

	public void FaceDirection (Coordinate direction)
	{
		transform.Rotate (new Vector3 (0, AngleFor (direction)) - transform.rotation.eulerAngles);
	}

	public void SetGoal(Coordinate coordinate)
	{
		Goal = coordinate;
	}

	// Returns true if this creature can reach the specified goal coordinate
	public bool CanReach(Coordinate goal)
	{
		IDictionary<Coordinate, Coordinate> parents;
		DoBFS(Position, goal, IsValidCoordinate, out parents);
		return parents.Keys.Contains(goal);
	}

	public bool HasAbility()
	{
		return GetComponent<IAbility>() != null;
	}

	public void UseAbility(Coordinate coordinate)
	{
		if (!HasAbility())
		{
			throw new InvalidOperationException("This creature does not have an ability");
		}
		Ability.Use(coordinate);
	}

	private float AngleFor(Coordinate direction)
	{
		if (direction == Coordinate.up) { return -90; }
		if (direction == Coordinate.right) { return 0; }
		if (direction == Coordinate.down) { return 90; }
		if (direction == Coordinate.left) { return 180; }
		throw new ArgumentException("Given coordinate" + direction + " is not a direction", "direction");
	}

	private delegate bool CoordinatePredicate(Coordinate coordinate);
	private static IDictionary<Coordinate, int> DoBFS(Coordinate start, Coordinate end, CoordinatePredicate neighborPredicate, out IDictionary<Coordinate, Coordinate> parents)
	{
		// Initialize BFS data structures
		var distance = new Dictionary<Coordinate, int>();
		var queue = new Queue<Coordinate> ();
		parents = new Dictionary<Coordinate, Coordinate>();

		// Input our start coordinate
		distance [start] = 0;
		queue.Enqueue (start);

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			var neighbors = current.CardinalNeighbors();
			foreach (Coordinate neighbor in neighbors)
			{
				if (!distance.ContainsKey(neighbor) && neighborPredicate(neighbor))
				{
					distance[neighbor] = distance[current] + 1;
					parents[neighbor] = current;
					queue.Enqueue(neighbor);
				}
			}
			// If we've already found the last coordinate, we do not need to keep searching.
			if (neighbors.Contains(end))
			{
				break;
			}
		}
		return distance;
	}

	// do a BFS and figure out the right path
	private Coordinate NextCoordinate()
	{
		// If the creature has no goal, just stick to the current position
		if (Goal == Position)
		{
			return Position;
		}

		IDictionary<Coordinate, Coordinate> parents;
		DoBFS(Position, Goal, IsValidCoordinate, out parents);

		Coordinate next;
		// If we can reach the goal, then move the creature to the next step towards that goal
		if (parents.ContainsKey(Goal))
		{
			next = Goal;
			while (parents[next] != Position)
			{
				next = parents[next];
			}
			return next;
		}

		// Otherwise, do another BFS not accounting for terrain restrictions and try to move the creature there
		var distance = DoBFS(Goal, Position, LevelManager.Terrain.Contains, out parents);
		// TODO assert that the level is fully connected
		foreach (var neighbor in Position.CardinalNeighbors())
		{
			if (distance.ContainsKey(neighbor) && distance[neighbor] < distance[Position] && IsValidCoordinate(neighbor))
			{
				return neighbor;
			}
		}
		return Position;
	}

	// Returns true if this creature is allowed to go to this coordinate
	private bool IsValidCoordinate(Coordinate coordinate)
	{
		return LevelManager.Terrain.Contains(coordinate)
			&& Definition.AllowedTerrain.Contains(LevelManager.Terrain[coordinate])
			&& !LevelManager.Creatures.CreatureList.Any(x => x != this && (x.Position == coordinate || x.NextPosition == coordinate));
	}

}
