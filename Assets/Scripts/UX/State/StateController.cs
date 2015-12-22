using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the selection of game objects and keeps track of what part of the
/// game is currently in focus.
/// </summary>
public class StateController : MonoBehaviour
{
	// TODO "SelectionController" is probably a more apt name.
	public CreatureSelector creatureSelector;
	public CreatureCreator creatureCreator;

	public CreatureSelector Selector { get { return creatureSelector; } }
	public CreatureCreator Creator { get { return creatureCreator; } }

	void Awake()
	{
		if (!creatureSelector) { creatureSelector = GetComponentInChildren<CreatureSelector>(); }
		if (!creatureCreator) { creatureCreator = GetComponentInChildren<CreatureCreator>(); }
	}

	// Use this for initialization
	void Start ()
	{
		// When we select a creature, we should stop creating
		creatureSelector.Selected += x => creatureCreator.StopCreation();
		// Deselect when we start creation
		creatureCreator.CreationStarted += x => creatureSelector.Deselect();
		// Select a creature if it's created
		creatureCreator.Created += creatureSelector.SelectCreature;

		// TODO Where *do* keyboard shortcuts go?
		UXManager.Input.KeyDown[KeyCode.Backspace] += creatureSelector.DestroySelectedCreature;
	}
}
