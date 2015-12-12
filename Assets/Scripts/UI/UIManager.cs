using UnityEngine;
using System.Collections;
using System.Linq;

// Main controller for all UI.
public class UIManager : MonoBehaviour
{

	public WinnerPanel winnerPanel;
	public InfoPanel infoPanel;
	public RecipeListPanel recipePanel;

	// Use this for initialization
	void Start ()
	{
		if (winnerPanel == null)
		{
			winnerPanel = GetComponentInChildren<WinnerPanel>();
		}
		GameManager.Creatures.OnSelect += DisplayCreatureInfo;
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
		infoPanel.Description = string.Format("Allowed Terrain: {0}",
			string.Join(", ", creatureDefinition.AllowedTerrain.Select(t => t.ToString()).ToArray()));
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
}
