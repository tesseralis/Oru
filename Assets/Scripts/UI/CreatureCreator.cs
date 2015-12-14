using UnityEngine;
using System;
using System.Collections;

public class CreatureCreator : MonoBehaviour {

	public GameObject createMarker;

	// Handler that is called when the creature is created
	public event Action<Creature> Created;

	private bool isCreating = false;
	private CreatureType currentCreatureType;

	public void StartCreation(CreatureType creature)
	{
		isCreating = true;
		currentCreatureType = creature;
		if (createMarker) { createMarker.SetActive(true); }
		GameManager.Terrain.MouseEnterBlock += MoveCreateMarker;
		GameManager.Terrain.ClickBlock += CreateCreature;
	}

	public void StopCreation()
	{
		isCreating = false;
		if (createMarker) { createMarker.SetActive(false); }
		GameManager.Terrain.MouseEnterBlock -= MoveCreateMarker;
		GameManager.Terrain.ClickBlock -= CreateCreature;
	}

	// Use this for initialization
	void Start () {
		if (createMarker) { createMarker.SetActive(false); }
	}

	void MoveCreateMarker(Coordinate coordinate)
	{
		if (createMarker != null && isCreating)
		{
			// Update the position visually
			GameManager.gm.SetPosition(createMarker, coordinate);
		}
	}

	void CreateCreature(Coordinate coordinate)
	{
		if (createMarker != null && isCreating)
		{
			if (GameManager.Creatures.CanCreateCreature(currentCreatureType, coordinate))
			{
				// If everything passes, add the creature to the list of creatures
				var creature = GameManager.Creatures.CreateCreature(currentCreatureType, coordinate);
				// We are no longer creating
				StopCreation();

				if (Created != null) { Created(creature); }
			}
		}
	}
}
