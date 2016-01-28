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

	public event Action<CreatureType> BlueprintEnter;
	public event Action BlueprintExit;

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
	}

	private void UpdateRecipeList(IList<CreatureType> availableRecipes)
	{
		// Don't show anything if we don't have any recipes.
		if (availableRecipes.Count <= 0)
		{
			gameObject.SetActive(false);
			return;
		}
		gameObject.SetActive(true);
		buttonContainer.DestroyAllChildren();

		var buttonSpacing = createButtonPrefab.GetComponent<RectTransform>().rect.height;

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

			createButton.MouseEnter += () => { if (BlueprintEnter != null) { BlueprintEnter(recipe); } };
			createButton.MouseExit += () => { if (BlueprintExit != null) { BlueprintExit(); } };
		}

		var sizeDelta = buttonContainer.GetComponent<RectTransform>().sizeDelta;
		buttonContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x, buttonSpacing * availableRecipes.Count);
	}
}
