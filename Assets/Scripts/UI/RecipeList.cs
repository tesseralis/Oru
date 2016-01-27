using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Util;

// This panel shows what recipes are available to be created
public class RecipeList : MonoBehaviour
{
	public GameObject buttonContainer;
	public GameObject createButtonPrefab;
	public RecipeInfo blueprintRecipeInfo;
	public RecipeInfo creationRecipeInfo;

	public float buttonSpacing = 30f;

	void Awake ()
	{
		var rectTransform = buttonContainer.GetComponent<RectTransform>();
		var anchoredPosition = rectTransform.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, 0);
	}

	// Use this for initialization
	void Start ()
	{
		LevelManager.Recipes.RecipesUpdated += UpdateRecipeList;
		UpdateRecipeList(LevelManager.Recipes.AvailableRecipes);

		// Show the recipe panel when the creature is actually being created
		UXManager.State.Creator.CreationStarted += creationRecipeInfo.DisplayRecipeInfo;
		UXManager.State.Creator.CreationStopped += creationRecipeInfo.HideRecipeInfo;
	}

	void UpdateRecipeList(IList<CreatureType> availableRecipes)
	{
		// Don't show anything if we don't have any recipes.
		if (availableRecipes.Count <= 0)
		{
			gameObject.SetActive(false);
			return;
		}
		gameObject.SetActive(true);
		buttonContainer.DestroyAllChildren();

		// Populate the UI with buttons corresponding with the recipes
		for (int i = 0; i < availableRecipes.Count; i++)
		{
			var recipe = availableRecipes[i];
			GameObject newButton = Instantiate(createButtonPrefab);
			newButton.transform.SetParent(buttonContainer.transform, false);
			newButton.GetComponentInChildren<Text>().text = recipe.ToString();
			// Translate the new object by the index amount
			var transform = newButton.GetComponent<RectTransform>();
			transform.anchoredPosition += Vector2.down * i * buttonSpacing;

			// Set the component to do the hooked in action when clicked
			var createButton = newButton.GetComponent<DelegateButton>();
			createButton.Click += () => UXManager.State.Creator.StartCreation(recipe);

			// Add mouse enter/exit events to display the selection panel
			createButton.MouseEnter += () => blueprintRecipeInfo.DisplayRecipeInfo(recipe);
			createButton.MouseExit += blueprintRecipeInfo.HideRecipeInfo;
		}

		var sizeDelta = buttonContainer.GetComponent<RectTransform>().sizeDelta;
		buttonContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x, buttonSpacing * availableRecipes.Count);
	}
}
