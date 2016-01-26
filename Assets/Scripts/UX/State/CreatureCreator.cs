using UnityEngine;
using System;
using System.Collections;
using Util;

public class CreatureCreator : MonoBehaviour {

	public GameObject createMarker;
	public GameObject positiveMarker;
	public GameObject negativeMarker;
	public GameObject creaturePreview;

	// Handler that is called when the creature is created
	public event Action<Creature> Created;
	// Handler that is called when creation has started
	public event Action<CreatureType> CreationStarted;
	// Handler that is called when creation has stopped
	public event Action CreationStopped;
	// Handler that is called when the user commits an error
	public event Action CreationError;

	private CreatureType currentCreatureType;
	private bool isCreating = false;

	public void StartCreation(CreatureType creature)
	{
		currentCreatureType = creature;

		// Ensure that we never double-click

		if (!isCreating)
		{
			isCreating = true;
			// Do the actual creation
			UXManager.Input.TerrainClicked += CreateCreature;
			UXManager.Input.DeselectButton += StopCreation;
		}

		if (CreationStarted != null) { CreationStarted(creature); }
	}

	public void StopCreation()
	{
		isCreating = false;
		if (createMarker) { createMarker.SetActive(false); }
		UXManager.Input.TerrainClicked -= CreateCreature;
		UXManager.Input.DeselectButton -= StopCreation;

		if (CreationStopped != null) { CreationStopped(); }
	}

	// Use this for initialization
	void Start ()
	{
		if (createMarker) { createMarker.SetActive(false); }
	}

	void Update()
	{
		var coordinate = UXManager.Input.CurrentCoordinate();
		if (isCreating && coordinate.HasValue)
		{
			ShowCreateMarker(coordinate.Value);
		}
		else
		{
			HideCreateMarker();
		}
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
				if (creaturePreview) { creaturePreview.SetActive(true); }
				var prefab = ResourcesPathfinder.CreaturePrefab(currentCreatureType);
				creaturePreview.DestroyAllChildren();
				creaturePreview.AddChild(prefab, coordinate);
			}
			else
			{		
				if (positiveMarker) { positiveMarker.SetActive(false); }
				if (negativeMarker) { negativeMarker.SetActive(true); }
				if (creaturePreview) { creaturePreview.SetActive(false); }
			}
		}
	}

	void HideCreateMarker()
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
		else
		{
			if (CreationError != null) { CreationError(); }
		}

	}
}
