using UnityEngine;
using System;
using System.Collections;

public class CreatureCreator : MonoBehaviour {

	public GameObject createMarker;
	public GameObject positiveMarker;
	public GameObject negativeMarker;

	// Handler that is called when the creature is created
	public event Action<Creature> Created;

	private CreatureType currentCreatureType;

	public void StartCreation(CreatureType creature)
	{
		currentCreatureType = creature;
		GameManager.Terrain.MouseEnterBlock += ShowCreateMarker;
		GameManager.Terrain.MouseExitBlock += HideCreateMarker;
		GameManager.Terrain.ClickBlock += CreateCreature;
	}

	public void StopCreation()
	{
		if (createMarker) { createMarker.SetActive(false); }
		GameManager.Terrain.MouseEnterBlock -= ShowCreateMarker;
		GameManager.Terrain.MouseExitBlock -= HideCreateMarker;
		GameManager.Terrain.ClickBlock -= CreateCreature;
	}

	// Use this for initialization
	void Start () {
		if (createMarker) { createMarker.SetActive(false); }
	}

	void ShowCreateMarker(Coordinate coordinate)
	{
		if (createMarker)
		{
			createMarker.SetActive(true);
			// Update the position visually
			GameManager.gm.SetPosition(createMarker, coordinate);
			if (GameManager.Creatures.CanCreateCreature(currentCreatureType, coordinate))
			{
				if (positiveMarker) { positiveMarker.SetActive(true); }
				if (negativeMarker) { negativeMarker.SetActive(false); }
			}
			else
			{		
				if (positiveMarker) { positiveMarker.SetActive(false); }
				if (negativeMarker) { negativeMarker.SetActive(true); }
			}
		}
	}

	void HideCreateMarker(Coordinate coordinate)
	{
		if (createMarker) { createMarker.SetActive(false); }
	}

	void CreateCreature(Coordinate coordinate)
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
