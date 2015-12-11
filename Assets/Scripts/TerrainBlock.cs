using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
		if (createMarker)
		{
			// Update the position visually
			GameManager.gm.SetPosition(createMarker, Coordinate);
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

		// Otherwise, if we're in "create" mode, create the creature specified under the current blueprints
		// TODO delegate this to a Controller class
		GameObject createMarker = GameManager.gm.createMarker;
		if (createMarker)
		{
			// TODO use the whole grid of nine blocks

			// TODO un-hardcode the recipe we need
			var currentRecipe = new Dictionary<ResourceType, int>();
			currentRecipe[ResourceType.Red] = 3; // we need three red for a dragon

			// TODO link the Dragon prefab from somewhere as part of the current recipe

			// TODO check if we have enough resources for the recipe

			// TODO if so, create a new instance of the creature in the prefab


		}
	}

}