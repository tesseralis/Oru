using UnityEngine;
using System.Linq;
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
				foreach (var marker in actionMarkers.GetComponentsInChildren<ActionMarker>())
				{
					marker.Action = value.GetComponent<IAction>();
				}
				actionMarkers.SetActive(false);
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
		if (Input.GetKeyDown("space") && (SelectedCreature != null))
		{
			Debug.LogFormat("Detected a space on {0}", SelectedCreature);
			// If the creature is moving, make it stop
			SelectedCreature.Goal = null;
			// Toggle the action marker
			// FIXME figure out why this won't work anymore.
			Debug.LogFormat("Creature is now set to {0}", actionMarkers.activeSelf);
			var activeValue = !actionMarkers.activeSelf;
			Debug.LogFormat("Setting activity to {0}", activeValue);
			actionMarkers.SetActive(activeValue);
		}
	}
}
