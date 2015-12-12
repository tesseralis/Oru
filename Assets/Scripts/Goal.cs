using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{

	// What creature is needed to win the game
	// TODO generalize this so this can be "any creature" or terrain (or boulders)
	public CreatureType winningCreatureType;

	// The info panel
	public InfoPanel infoPanel;

	void OnMouseDown()
	{
		// Disable creature markers
		GameManager.Creatures.SelectedCreature = null;
		// Update the info UI
		infoPanel.Name = "Goal";
		infoPanel.Description = winningCreatureType + " at this location.";
	}

}

