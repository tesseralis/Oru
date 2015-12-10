using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{

	// What creature is needed to win the game
	public CreatureType winningCreatureType;

	// The info panel
	public InfoPanel infoPanel;

	public Coordinate Coordinate
	{
		get { return GameManager.gm.ToGridCoordinate(transform.position); }
	}

	void OnMouseDown()
	{
		// Disable creature markers
		GameManager.gm.creatures.SelectedCreature = null;
		// Update the info UI
		infoPanel.Name = "Goal";
		infoPanel.Description = winningCreatureType + " at this location.";
	}

}

