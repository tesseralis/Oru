using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RecipeListPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	// TODO Use delegates
	void Update () {
		var recipes = GameManager.Recipes;
		var availableRecipes = recipes.AvailableRecipes;

		var buttons = GetComponentsInChildren<Button>(true);

		for (int i = 0; i < availableRecipes.Count; i++)
		{
			var recipe = availableRecipes[i];
			var button = buttons[i];

			// TODO figure out how to dynamically create buttons
			button.gameObject.SetActive(true);
			button.GetComponentInChildren<Text>().text = recipe.ToString();
			// TODO Make this a default method from the recipe manager
			button.onClick.AddListener(() =>
			{
					recipes.IsCreating = true;
					recipes.CurrentRecipe = recipe;
			});
		}
		for (int i = availableRecipes.Count; i < buttons.Length; i++)
		{
			var button = buttons[i];
			button.gameObject.SetActive(false);
		}
	}
}
