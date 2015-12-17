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
	public InfoPanel selectionInfoPanel;
	public CoordinateInfo coordinateInfo;
	public RecipeListPanel recipePanel;

	public EntitySelector entitySelector;
	public CreatureCreator creatureCreator;

	// TODO move this and the corresponding function to a different class
	public float cameraTranslateFactor = 0.25f;

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
		if (recipePanel == null)
		{
			recipePanel = GetComponentInChildren<RecipeListPanel>();
		}
	}

	// Use this for initialization
	void Start ()
	{
		GameManager.Terrain.MouseEnterBlock += coordinateInfo.Show;
		GameManager.Terrain.MouseExitBlock += x => coordinateInfo.Hide();

		// Deselect creatures when we start creation
		recipePanel.RecipeClicked += (t) => entitySelector.Deselect();
		// Start creating the recipe
		recipePanel.RecipeClicked += creatureCreator.StartCreation;

		GameManager.gm.OnWin += DisplayWinInfo;

		// When we select a creature, we should stop creating
		entitySelector.Select += x => creatureCreator.StopCreation();
		entitySelector.Select += DisplayCreatureInfo;

		// Select a creature if it's created
		creatureCreator.Created += entitySelector.SelectCreature;

		// Add map controls
		GameManager.Input.Key[KeyCode.DownArrow] += () => TranslateCamera(Vector2.down);
		GameManager.Input.Key[KeyCode.UpArrow] += () => TranslateCamera(Vector2.up);
		GameManager.Input.Key[KeyCode.LeftArrow] += () => TranslateCamera(Vector2.left);
		GameManager.Input.Key[KeyCode.RightArrow] += () => TranslateCamera(Vector2.right);

	}

	void DisplayWinInfo()
	{
		winnerPanel.gameObject.SetActive(true);
		selectionInfoPanel.gameObject.SetActive(false);
		recipePanel.gameObject.SetActive(false);

		coordinateInfo.Hide();

		// TODO disable all events
	}

	void DisplayCreatureInfo(Creature creature)
	{
		selectionInfoPanel.gameObject.SetActive(true);
		selectionInfoPanel.Name = creature.creatureType.ToString();
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

		selectionInfoPanel.Description = string.Format("Allowed Terrain: {0}\nAbility: {1}",
			string.Join(", ", creatureDefinition.AllowedTerrain.Select(t => t.ToString()).ToArray()),
			ability);
	}

	// Pan our camera in the specified direction
	void TranslateCamera(Vector2 direction)
	{
		var camera = GameObject.FindGameObjectWithTag("MainCamera");
		camera.transform.Translate(new Vector3(direction.x, direction.y, direction.x) * cameraTranslateFactor);
	}
}
