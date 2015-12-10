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
		infoPanel.gameObject.SetActive(true);
		infoPanel.Name = "Goal";
		infoPanel.Description = winningCreatureType + " at this location.";
	}

}

