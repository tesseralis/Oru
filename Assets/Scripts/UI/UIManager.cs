using UnityEngine;
using System.Collections;
using System.Linq;
using Util;

// Main controller for all UI.
public class UIManager : MonoBehaviour
{

	public WinnerPanel winnerPanel;
	public InfoPanel infoPanel;
	public RecipeListPanel recipePanel;

	void Awake ()
	{
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
		else
		{
			ability = "None";
		}

		infoPanel.Description = string.Format("Allowed Terrain: {0}\nAbility: {1}",
			string.Join(", ", creatureDefinition.AllowedTerrain.Select(t => t.ToString()).ToArray()),
			ability);
	}

	void DisplayGoalInfo(CreatureType type)
	{
		// Disable creature markers
		// TODO Where does this go?
		GameManager.Creatures.SelectedCreature = null;
		// Update the info UI
		infoPanel.gameObject.SetActive(true);
		infoPanel.Name = "Goal";
		infoPanel.Description = type + " at this location.";
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
}
