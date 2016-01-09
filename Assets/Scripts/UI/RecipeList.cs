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
	public RecipeInfo recipeInfo;

	public float buttonSpacing = 30f;

	// Use this for initialization
	void Start ()
	{
		LevelManager.Recipes.RecipesUpdated += UpdateRecipeList;
		UpdateRecipeList(LevelManager.Recipes.AvailableRecipes);
	}

	void UpdateRecipeList(IList<CreatureType> availableRecipes)
	{
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
			createButton.MouseEnter += () => ShowRecipeInfoPanel(recipe);
			createButton.MouseExit += HideRecipeInfoPanel;
		}
	}

	void ShowRecipeInfoPanel(CreatureType recipe)
	{
		recipeInfo.gameObject.SetActive(true);
		recipeInfo.DisplayRecipeInfo(recipe);
	}

	void HideRecipeInfoPanel()
	{
		recipeInfo.gameObject.SetActive(false);
	}
}
