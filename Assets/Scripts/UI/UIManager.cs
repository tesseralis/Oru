using UnityEngine;
using UnityEngine.UI;
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

	public EntitySelector entitySelector;
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
		GameManager.Terrain.MouseEnterBlock += coordinateInfo.Show;
		GameManager.Terrain.MouseExitBlock += x => coordinateInfo.Hide();

		// Deselect creatures when we start creation
		recipeList.RecipeClicked += (t) => entitySelector.Deselect();
		// Start creating the recipe
		recipeList.RecipeClicked += creatureCreator.StartCreation;

		GameManager.gm.OnWin += DisplayWinInfo;

		// When we select a creature, we should stop creating
		entitySelector.Select += x => creatureCreator.StopCreation();
		entitySelector.Select += creatureInfo.DisplayCreatureInfo;

		// Select a creature if it's created
		creatureCreator.Created += entitySelector.SelectCreature;

		// Destroy creature on key shortcut
		// TODO this should be a button on the creature info panel
		GameManager.Input.KeyDown[KeyCode.Backspace] += () =>
		{
			var creature = entitySelector.SelectedCreature;
			entitySelector.Deselect();
			GameManager.Creatures.DestroyCreature(creature);
		};

	}

	// TODO make the win text an in-world panel like the original game?
	void DisplayWinInfo()
	{
		winnerPanel.gameObject.SetActive(true);
		creatureInfo.gameObject.SetActive(false);
		recipeList.gameObject.SetActive(false);

		coordinateInfo.Hide();

		// TODO disable all events
	}

}
