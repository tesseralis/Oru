using UnityEngine;
using System;
using System.Collections;

public class Goal : MonoBehaviour
{

	// What creature is needed to win the game
	// TODO generalize this so this can be "any creature" or terrain (or boulders)
	public CreatureType winningCreatureType;

	// The info panel
	public InfoPanel infoPanel;

	public Action<CreatureType> OnClick;

	void OnMouseDown()
	{
		if (OnClick != null) { OnClick(winningCreatureType); }
	}

}

