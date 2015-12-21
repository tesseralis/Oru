using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;

// Main controller for all UI.
public class UIManager : MonoBehaviour
{
	// make game manager accessible throughout the game
	public static UIManager ui;

	public WinnerPanel winnerPanel;
	public CreatureInfo creatureInfo;
	public CoordinateInfo coordinateInfo;
	public RecipeList recipeList;
	public GameObject menuPanel;


	void Awake ()
	{
		if (ui == null)
		{
			ui = this.gameObject.GetComponent<UIManager>();
		}
		// Initialize our child components
		if (winnerPanel == null)
		{
			winnerPanel = GetComponentInChildren<WinnerPanel>();
		}
		if (recipeList == null)
		{
			recipeList = GetComponentInChildren<RecipeList>();
		}
	}

	// Use this for initialization
	void Start ()
	{
		LevelManager.Terrain.MouseEnterBlock += coordinateInfo.Show;
		LevelManager.Terrain.MouseExitBlock += x => coordinateInfo.Hide();

		// Start creating the recipe
		// TODO propagate this to the recipe list class itself
		recipeList.RecipeClicked += UXManager.State.Creator.StartCreation;

		LevelManager.level.OnWin += DisplayWinInfo;

	// When we deselect a creature, we should hide the info panel
		UXManager.State.Selector.Deselected += creatureInfo.HideCreatureInfo;
		UXManager.State.Selector.Selected += creatureInfo.DisplayCreatureInfo;


		// Add handlers for using ability
		// TODO disable this when you the selected creature does not have an ability
		creatureInfo.useAbilityButton.Click += UXManager.State.Selector.actionMarkers.ToggleAbility;

		// Add handlers for destroying the creature
		creatureInfo.destroyCreatureButton.Click += UXManager.State.Selector.DestroySelectedCreature;
	}

	// TODO make the win text an in-world panel like the original game?
	void DisplayWinInfo()
	{
		winnerPanel.gameObject.SetActive(true);
		creatureInfo.gameObject.SetActive(false);
		recipeList.gameObject.SetActive(false);
		menuPanel.gameObject.SetActive(false);

		coordinateInfo.Hide();
	}

}


