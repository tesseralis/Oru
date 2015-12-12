using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RecipeListPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var recipes = GameManager.gm.recipes.AvailableRecipes;

		var buttons = GetComponentsInChildren<Button>(true);

		for (int i = 0; i < recipes.Count; i++)
		{
			var recipe = recipes[i];
			var button = buttons[i];

			// TODO figure out how to dynamically create buttons
			button.gameObject.SetActive(true);
			button.GetComponentInChildren<Text>().text = recipe.ToString();
			// TODO Make this a default method from the recipe manager
			button.onClick.AddListener(() =>
			{
					GameManager.gm.recipes.IsCreating = true;
					GameManager.gm.recipes.CurrentRecipe = recipe;
					// TODO another space where we'd benefit from a "deselect all" method
					GameManager.gm.creatures.SelectedCreature = null;
					GameManager.gm.creatures.IsActing = false;
			});
		}
		for (int i = recipes.Count; i < buttons.Length; i++)
		{
			var button = buttons[i];
			button.gameObject.SetActive(false);
		}
	}
}
