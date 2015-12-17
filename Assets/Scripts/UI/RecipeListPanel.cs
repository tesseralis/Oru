using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// This panel shows what recipes are available to be created
public class RecipeListPanel : MonoBehaviour
{

	public event Action<CreatureType> RecipeClicked;

	// Use this for initialization
	void Start ()
	{
		GameManager.Recipes.OnChange += UpdateRecipeList;
		UpdateRecipeList(GameManager.Recipes.availableRecipes);
	}

	void UpdateRecipeList(IList<CreatureType> availableRecipes)
	{
		gameObject.SetActive(true);

		var buttons = GetComponentsInChildren<Button>(true);
		for (int i = 0; i < availableRecipes.Count; i++)
		{
			var recipe = availableRecipes[i];
			var button = buttons[i];

			// TODO figure out how to dynamically create buttons
			button.gameObject.SetActive(true);
			button.GetComponentInChildren<Text>().text = recipe.ToString();
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => RecipeClicked(recipe));
//			button.onClick.AddListener(() =>
//				{
//					// Display the information
//					// FIXME do this on mouse hover instead
//					selectionInfoPanel.gameObject.SetActive(true);
//					selectionInfoPanel.Name = recipe.ToString();
//					selectionInfoPanel.Description = string.Join("\n",
//						Creatures.ForType(recipe).Recipe.Select(e => e.Key + ": " + e.Value).ToArray());
//				});

		}
		for (int i = availableRecipes.Count; i < buttons.Length; i++)
		{
			Debug.LogFormat("Disabling button at index {0}", i);
			buttons[i].gameObject.SetActive(false);
		}
	}
}
