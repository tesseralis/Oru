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

	public EntitySelector entitySelector;
	public CreatureCreator creatureCreator;

	// TODO these should be factored out to a separate Audio class
	public SoundEffectOptions sfxOptions;

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
		GameManager.Input.KeyDown[KeyCode.Backspace] += DestroySelectedCreature;
		creatureInfo.destroyCreatureButton.Click += DestroySelectedCreature;

		// Play sounds when the creature takes actions
		GameManager.Creatures.CreatureCreated += x => PlaySound(sfxOptions.createAudio);
		GameManager.Creatures.CreatureDestroyed += () => PlaySound(sfxOptions.destroyAudio);
		entitySelector.Selected += x => PlaySound(sfxOptions.creatureSelectAudio);
		entitySelector.GoalSet += (x, y) => PlaySound(sfxOptions.setCreatureGoalAudio);
		GameManager.Recipes.RecipesUpdated += (obj) => PlaySound(sfxOptions.pickupRecipeAudio);
		entitySelector.actionMarkers.AbilityUsed += () => PlaySound(sfxOptions.useAbilityAudio);
	}

	void DestroySelectedCreature()
	{
		var creature = entitySelector.SelectedCreature;
		entitySelector.Deselect(); // TODO this deselect method goes somewhere else?
		GameManager.Creatures.DestroyCreature(creature);
	}

	// TODO make the win text an in-world panel like the original game?
	void DisplayWinInfo()
	{
		winnerPanel.gameObject.SetActive(true);
		creatureInfo.gameObject.SetActive(false);
		recipeList.gameObject.SetActive(false);
		menuPanel.gameObject.SetActive(false);

		coordinateInfo.Hide();

		// TODO disable all events
	}

	// Play the given sound
	void PlaySound(AudioClip clip)
	{
		if (clip)
		{
			Camera.main.GetComponent<AudioSource>().PlayOneShot(clip);
		}
	}

}

[Serializable]
public class SoundEffectOptions
{
	public AudioClip destroyAudio;
	public AudioClip createAudio;
	public AudioClip creatureSelectAudio;
	public AudioClip pickupRecipeAudio;
	public AudioClip setCreatureGoalAudio;
	// TODO separate audio for different abilities
	public AudioClip useAbilityAudio;
}
