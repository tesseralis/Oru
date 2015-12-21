using UnityEngine;
using System;
using System.Collections;
using Util;

public class CreatureCreator : MonoBehaviour {

	public GameObject createMarker;
	public GameObject positiveMarker;
	public GameObject negativeMarker;

	// Handler that is called when the creature is created
	public event Action<Creature> Created;
	// Handler that is called when creation has started
	public event Action<CreatureType> CreationStarted;
	// Handler that is called when creation has stopped
	public event Action CreationStopped;

	private CreatureType currentCreatureType;

	public void StartCreation(CreatureType creature)
	{
		// Ensure that we never double-click
		// TODO refactor this to store an internal "is-creating" state
		StopCreation();

		// Do the actual creation
		currentCreatureType = creature;
		LevelManager.Terrain.MouseEnterBlock += ShowCreateMarker;
		LevelManager.Terrain.MouseExitBlock += HideCreateMarker;
		LevelManager.Terrain.ClickBlock += CreateCreature;

		if (CreationStarted != null) { CreationStarted(creature); }
	}

	public void StopCreation()
	{
		if (createMarker) { createMarker.SetActive(false); }
		LevelManager.Terrain.MouseEnterBlock -= ShowCreateMarker;
		LevelManager.Terrain.MouseExitBlock -= HideCreateMarker;
		LevelManager.Terrain.ClickBlock -= CreateCreature;

		if (CreationStopped != null) { CreationStopped(); }
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
			createMarker.SetPosition(coordinate);
			if (LevelManager.Creatures.CanCreateCreature(currentCreatureType, coordinate))
			{
				if (positiveMarker) { positiveMarker.SetActive(true); }
				if (negativeMarker) { negativeMarker.SetActive(false); }
				// TODO fix the creature preview
//				var prefab = GameManager.Creatures.PrefabFor(currentCreatureType);
//				GameObject newObject = GameObject.Instantiate(prefab);
//				newObject.transform.SetParent(createMarker.transform, false);
//				newObject.GetComponent<BoxCollider>().enabled = false;
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
		if (createMarker) {
			// Destroy the mock creature we made
			if (createMarker.GetComponentInChildren<Creature>())
			{
				Destroy(createMarker.GetComponentInChildren<Creature>().gameObject);
			}
			createMarker.SetActive(false);
		}
	}

	void CreateCreature(Coordinate coordinate)
	{
		if (LevelManager.Creatures.CanCreateCreature(currentCreatureType, coordinate))
		{
			// If everything passes, add the creature to the list of creatures
			var creature = LevelManager.Creatures.CreateCreature(currentCreatureType, coordinate);
			// We are no longer creating
			StopCreation();
		
			if (Created != null) { Created(creature); }
		}

	}
}
