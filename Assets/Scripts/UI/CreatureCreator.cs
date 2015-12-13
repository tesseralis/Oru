using UnityEngine;
using System.Collections;

public class CreatureCreator : MonoBehaviour {

	public GameObject createMarker;

	private bool isCreating = false;
	private CreatureType currentCreatureType;

	public void StartCreation(CreatureType creature)
	{
		isCreating = true;
		currentCreatureType = creature;
		if (createMarker) { createMarker.SetActive(true); }
		GameManager.Terrain.OnHover += MoveCreateMarker;
		GameManager.Terrain.OnClick += CreateCreature;

		// TODO Deselect all other UI
	}

	public void StopCreation()
	{
		isCreating = false;
		if (createMarker) { createMarker.SetActive(false); }
		GameManager.Terrain.OnHover -= MoveCreateMarker;
		GameManager.Terrain.OnClick -= CreateCreature;
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
				GameManager.Creatures.CreateCreature(currentCreatureType, coordinate);
			}
		}
		// We are no longer creating
		StopCreation();
	}
}
