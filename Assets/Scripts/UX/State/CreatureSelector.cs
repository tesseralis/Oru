using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// The entity selector is responsible for the visual display of when something
/// is "selected" and acting.
/// </summary>
public class CreatureSelector : MonoBehaviour
{
	public GameObject creatureMarker;
	public ActionMarkers actionMarkers;

	public event Action<Creature> Selected;
	public event Action Deselected;
	public event Action<Creature, Coordinate> GoalSet;
	public event Action AbilityUsed;

	public Creature SelectedCreature { get; private set; }

	void Awake ()
	{
		if (actionMarkers == null)
		{
			actionMarkers = GetComponentInChildren<ActionMarkers>();
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (creatureMarker) { creatureMarker.SetActive(false); }
		actionMarkers.Disable();
		UXManager.Input.CreatureClicked += SelectCreature;
		LevelManager.Creatures.CreatureDestroyed += (x) => Deselect();
	}

	public void SelectCreature(Creature creature)
	{
		if (SelectedCreature == null)
		{
			// Make the entity marker visible
			if (creatureMarker) { creatureMarker.SetActive(true); }
			// Add a listener for terrain clicks
			UXManager.Input.TerrainClicked += OnClickBlock;
		}
		SelectedCreature = creature;

		// Make this a child of the object
		transform.SetParent(creature.transform, false);

		// Add a listener to the action markers
		if (creature.HasAbility())
		{
			actionMarkers.Enable(creature);
		}
		else
		{
			actionMarkers.Disable();
		}

		// Run the events in the handler
		if (Selected != null) { Selected(creature); }
	}

	public void DestroySelectedCreature()
	{
		LevelManager.Creatures.DestroyCreature(SelectedCreature);
	}

	public void Deselect()
	{
		UXManager.Input.TerrainClicked -= OnClickBlock;
		actionMarkers.Disable();
		SelectedCreature = null;
		transform.SetParent(null, false);

		if (creatureMarker) { creatureMarker.SetActive(false); }

		// Run the events in the handler
		if (Deselected != null) { Deselected(); }
	}

	private void OnClickBlock(Coordinate coordinate)
	{
		if (actionMarkers.IsActing)
		{
			UseCreatureAbility(coordinate);
		}
		else
		{
			SetCurrentCreatureGoal(coordinate);
		}
	}

	private void UseCreatureAbility(Coordinate coordinate)
	{
		// TODO verify if we can use the ability here
		SelectedCreature.UseAbility(coordinate);
		actionMarkers.StopAbility();
		if (AbilityUsed != null) { AbilityUsed(); }
	}
		
	private void SetCurrentCreatureGoal(Coordinate coordinate)
	{
		if (SelectedCreature.CanReach(coordinate))
		{
			SelectedCreature.SetGoal(coordinate);
			if (GoalSet != null) { GoalSet(SelectedCreature, coordinate); }
		}
	}

}
