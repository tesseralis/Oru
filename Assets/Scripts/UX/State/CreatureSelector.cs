using UnityEngine;
using System;
using System.Collections;
using System.Linq;

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
	// Called when the user commits an error on a creature
	public event Action<Creature> CreatureError;

	public Creature SelectedCreature { get; private set; }

	void Awake ()
	{
		if (actionMarkers == null)
		{
			actionMarkers = GetComponentInChildren<ActionMarkers>();
		}
		LevelManager.LevelLoaded += OnLevelLoad;
	}

	// Use this for initialization
	void Start ()
	{
		if (creatureMarker) { creatureMarker.SetActive(false); }
		actionMarkers.Disable();
		UXManager.Input.TerrainClicked += OnClickBlock;

	}

	void OnLevelLoad(LevelManager level)
	{
		LevelManager.Creatures.CreatureDestroyed += (x, pos) => 
		{
			if (x == SelectedCreature)
			{
				Deselect();
			}
		};
		var firstCreature = LevelManager.Creatures.CreatureList.FirstOrDefault(x => !x.Definition.IsEnemy);
		if (firstCreature)
		{
			SelectCreature(firstCreature);
		}
	}

	public void SelectCreature(Creature creature)
	{
		if (SelectedCreature == null)
		{
			// Make the entity marker visible
			if (creatureMarker) { creatureMarker.SetActive(true); }
			// Add a listener for terrain clicks
			UXManager.Input.DeselectButton += Deselect;
		}
		SelectedCreature = creature;

		// Make this a child of the object
		transform.SetParent(creature.transform, false);

		// Add a listener to the action markers
		if (!creature.Definition.IsEnemy && creature.HasAbility())
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
		UXManager.Input.DeselectButton -= Deselect;
		actionMarkers.Disable();
		SelectedCreature = null;
		transform.SetParent(null, false);

		if (creatureMarker) { creatureMarker.SetActive(false); }

		// Run the events in the handler
		if (Deselected != null) { Deselected(); }
	}

	// TODO refactor this into a method in the ability list
	private bool CanAttack(Coordinate coordinate)
	{
		var fight = SelectedCreature.GetComponent<FightAbility>();
		return fight && fight.CanUse(coordinate) && !SelectedCreature.Definition.IsEnemy;
	}

	private void OnClickBlock(Coordinate coordinate)
	{
		if (SelectedCreature && (actionMarkers.isActing || CanAttack(coordinate)))
		{
			UseCreatureAbility(coordinate);
		}
		else if (LevelManager.Creatures[coordinate])
		{
			SelectCreature(LevelManager.Creatures[coordinate]);
		}
		else if (SelectedCreature)
		{
			SetCurrentCreatureGoal(coordinate);
		}
	}

	private void UseCreatureAbility(Coordinate coordinate)
	{
		if (SelectedCreature.CanUseAbility(coordinate))
		{
			SelectedCreature.UseAbility(coordinate);
			actionMarkers.StopAbility();
			if (AbilityUsed != null) { AbilityUsed(); }
		}
		else
		{
			if (CreatureError != null) { CreatureError(SelectedCreature); }
		}
	}
		
	private void SetCurrentCreatureGoal(Coordinate coordinate)
	{
		if (SelectedCreature.CanReach(coordinate))
		{
			SelectedCreature.SetGoal(coordinate);
			if (GoalSet != null) { GoalSet(SelectedCreature, coordinate); }
			if (SelectedCreature.HasAbility())
			{
				SelectedCreature.Ability.Cancel();
			}
		}
		else
		{
			if (CreatureError != null) { CreatureError(SelectedCreature); }
		}
	}

}
