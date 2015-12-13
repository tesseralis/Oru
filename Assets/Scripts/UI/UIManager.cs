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
	public InfoPanel infoPanel;
	public RecipeListPanel recipePanel;

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
		if (infoPanel == null)
		{
			infoPanel = GetComponentInChildren<InfoPanel>();
		}
		if (recipePanel == null)
		{
			recipePanel = GetComponentInChildren<RecipeListPanel>();
		}
	}

	// Use this for initialization
	void Start ()
	{
		// Add listeners to the necessary game objects.
		GameManager.Creatures.OnSelect += DisplayCreatureInfo;
		GameManager.Terrain.OnHover += DisplayPileInfo;
		GameManager.Recipes.OnChange += UpdateRecipeList;
		GameManager.gm.goal.OnClick += DisplayGoalInfo;
		GameManager.gm.OnWin += DisplayWinInfo;
	}

	void DisplayWinInfo()
	{
		winnerPanel.gameObject.SetActive(true);
		infoPanel.gameObject.SetActive(false);
		recipePanel.gameObject.SetActive(false);
	}

	void DisplayCreatureInfo(Creature creature)
	{
		infoPanel.gameObject.SetActive(true);
		infoPanel.Name = creature.creatureType.ToString();
		var creatureDefinition = Creatures.ForType(creature.creatureType);
		// TODO generalize for all abilities!
		string ability;
		if (creature.GetComponent<ChangeTerrainAbility>() != null)
		{
			ability = string.Format("Pick up {0}\n\nPress <Space> to activate.",
				creature.GetComponent<ChangeTerrainAbility>().carryType);
		}
		else if (creature.GetComponent<CarryResourceAbility>() != null)
		{
			ability = string.Format("Pick up {0} resources\n\nPress <Space> to activate.",
				creature.GetComponent<CarryResourceAbility>().capacity);
		}
		else
		{
			ability = "None";
		}

		infoPanel.Description = string.Format("Allowed Terrain: {0}\nAbility: {1}",
			string.Join(", ", creatureDefinition.AllowedTerrain.Select(t => t.ToString()).ToArray()),
			ability);
	}

	void DisplayGoalInfo(Goal goal)
	{
		// Update the info UI
		infoPanel.gameObject.SetActive(true);
		infoPanel.Name = "Goal";
		infoPanel.Description = goal.winningCreatureType + " at this location.";
	}

	void DisplayPileInfo(Coordinate coordinate)
	{
		if (!GameManager.Resources[coordinate].IsEmpty())
		{
			infoPanel.gameObject.SetActive(true);
			infoPanel.Name = "Resource Pile";
			infoPanel.Description = string.Join("\n",
				GameManager.Resources[coordinate].Select(e => e.Key + ": " + e.Value).ToArray());
		}
	}

	// TODO some of this code should be moved back to the enclosing object.
	void UpdateRecipeList(IList<CreatureType> availableRecipes)
	{
		recipePanel.gameObject.SetActive(true);

		var buttons = recipePanel.GetComponentsInChildren<Button>(true);
		for (int i = 0; i < availableRecipes.Count; i++)
		{
			Debug.LogFormat("Updating button at index {0}", i);
			var recipe = availableRecipes[i];
			var button = buttons[i];

			// TODO figure out how to dynamically create buttons
			button.gameObject.SetActive(true);
			button.GetComponentInChildren<Text>().text = recipe.ToString();
			// TODO Make this a default method from the recipe manager
			// TODO doesn't this add multiple listeners?
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() =>
				{
					entitySelector.Deselect();
					creatureCreator.StartCreation(recipe);

					// Display the information
					// TODO make this a separate listener
					// TODO do this on mouse hover instead
					infoPanel.Name = recipe.ToString();
					infoPanel.Description = string.Join("\n",
						Creatures.ForType(recipe).Recipe.Select(e => e.Key + ": " + e.Value).ToArray());
				});

		}
		for (int i = availableRecipes.Count; i < buttons.Length; i++)
		{
			Debug.LogFormat("Disabling button at index {0}", i);
			buttons[i].gameObject.SetActive(false);
		}
	}
}
