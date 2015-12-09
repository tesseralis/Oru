using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Manages interactions between creatures and the rest of the system.
/// </summary>
public class CreatureManager : MonoBehaviour
{

	public GameObject creatureMarker;

	public GameObject actionMarkers;

	private Creature selectedCreature;

	public IList<Creature> Creatures
	{
		get
		{
			return new List<Creature>(GetComponentsInChildren<Creature>());
		}
	}

	public Creature SelectedCreature
	{
		get
		{
			return selectedCreature;
		}
		set
		{
			selectedCreature = value;
			if (creatureMarker)
			{
				Debug.LogFormat("Setting creature marker of {0} to {1}", selectedCreature, creatureMarker);
				creatureMarker.SetActive(true);
				creatureMarker.transform.SetParent(value.gameObject.transform, false);
			}
			if (actionMarkers)
			{
				Debug.LogFormat("Setting action markers of {0} to {1}", selectedCreature, actionMarkers);
				actionMarkers.transform.SetParent(value.gameObject.transform, false);
				actionMarkers.GetComponent<ActionMarkers>().Action = value.GetComponent<IAction>();
			}
		}
	}

	/// <summary>
	/// Move all the creatures forward one game step.
	/// </summary>
	public void Step()
	{
		foreach(Creature creature in Creatures)
		{
			creature.Step();
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown("space") && SelectedCreature != null)
		{
			// If the creature is moving, make it stop
			SelectedCreature.Goal = null;
			// Toggle the action marker
			actionMarkers.SetActive(!actionMarkers.activeInHierarchy);
		}
	}
}
