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

	public CreatureSelector entitySelector;
	public CreatureCreator creatureCreator;

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

		// Deselect creatures when we start creation
		recipeList.RecipeClicked += (t) => entitySelector.Deselect();
		// Start creating the recipe
		recipeList.RecipeClicked += creatureCreator.StartCreation;

		LevelManager.level.OnWin += DisplayWinInfo;

		// When we select a creature, we should stop creating
		entitySelector.Selected += x => creatureCreator.StopCreation();
		entitySelector.Selected += creatureInfo.DisplayCreatureInfo;

		// When we deselect a creature, we should hide the info panel
		entitySelector.Deselected += creatureInfo.HideCreatureInfo;

		// Select a creature if it's created
		creatureCreator.Created += entitySelector.SelectCreature;

		// Add handlers for using ability
		// TODO disable this when you the selected creature does not have an ability
		creatureInfo.useAbilityButton.Click += entitySelector.actionMarkers.ToggleAbility;

		// Add handlers for destroying the creature
		UXManager.Input.KeyDown[KeyCode.Backspace] += entitySelector.DestroySelectedCreature;
		creatureInfo.destroyCreatureButton.Click += entitySelector.DestroySelectedCreature;
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


