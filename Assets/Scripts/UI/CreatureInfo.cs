using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Util;

public class CreatureInfo : MonoBehaviour
{
	public GameObject panel;
	public Text nameDisplay;
	public GameObject blueprintLabel;
	public GameObject healthInfo;
	public Healthbar healthbar;
	public Text descriptionDisplay;
	public ResourceList resourceList;
	public Button useAbilityButton;
	public Button destroyCreatureButton;

	public RecipeList recipeList;

	private Creature selectedCreature;
	private CreatureType? creatingCreature;
	private CreatureType? highlightedBlueprint;

	public void Start()
	{
		UXManager.State.Selector.Selected += (x) => { selectedCreature = x; };
		UXManager.State.Selector.Deselected += () => { selectedCreature = null; };

		UXManager.State.Creator.CreationStarted += (x) => { creatingCreature = x; };
		UXManager.State.Creator.CreationStopped += () => { creatingCreature = null; };

		recipeList.BlueprintEnter += (x) => { highlightedBlueprint = x; };
		recipeList.BlueprintExit += () => { highlightedBlueprint = null; };

		panel.SetActive(false);
	}

	void Update()
	{
		if (highlightedBlueprint.HasValue)
		{
			DisplayRecipeInfo(highlightedBlueprint.Value);
		}
		else if (creatingCreature.HasValue)
		{
			DisplayRecipeInfo(creatingCreature.Value);
		}
		else if (selectedCreature)
		{
			DisplayCreatureInfo(selectedCreature);
		}
		else
		{
			panel.SetActive(false);
		}
	}

	private void DisplayRecipeInfo(CreatureType blueprint)
	{
		DisplayInfo(blueprint);

		// Hide creature health
		healthInfo.gameObject.SetActive(false);

		// display "blueprint" subtitle
		blueprintLabel.SetActive(true);

		// Hide the ability and destroy buttons
		useAbilityButton.gameObject.SetActive(false);
		destroyCreatureButton.gameObject.SetActive(false);
	}

	private void DisplayCreatureInfo(Creature creature)
	{
		DisplayInfo(creature.creatureType);

		// Display creature health
		healthInfo.gameObject.SetActive(true);
		healthbar.SetHealth(creature.health);

		// Hide "blueprint" label
		blueprintLabel.SetActive(false);

		// Display creature data
		var creatureDefinition = CreatureDefinition.ForType(creature.creatureType);
		useAbilityButton.gameObject.SetActive(!creature.Definition.IsEnemy && creature.HasAbility());

		// Display the creature's resources
		resourceList.ShowResources(creatureDefinition.RecipeWithEnergy());

		DisplayAbilityButtonText(creature);
		var isEnemy = creature.Definition.IsEnemy;
		destroyCreatureButton.gameObject.SetActive(!isEnemy);
	}

	// Display info common to both creatures and blueprints
	private void DisplayInfo(CreatureType creatureType)
	{
		panel.SetActive(true);
		var definition = CreatureDefinition.ForType(creatureType);

		// Display the creature's name
		nameDisplay.text = creatureType.ToString();

		// display the information in the recipe
		var text = "Allowed Terrain: " + string.Join(", ", definition.AllowedTerrain.Select(t => t.ToString()).ToArray());
		if (definition.Ability != null)
		{
			text += "\nAbility: " + definition.Ability.Description();
		}
		// TODO makes this more typesafe?
		if (definition.Ability is FightAbility.Definition)
		{
			var ability = (FightAbility.Definition)definition.Ability;
			text += "\nAttack: " + ability.Attack;
			text += "\nDefense: " + ability.Defense;
		}
		descriptionDisplay.text = text;

	}

	private void DisplayAbilityButtonText(Creature creature)
	{
		var text = useAbilityButton.GetComponentInChildren<Text>();
		if (creature.HasAbility())
		{
			text.text = creature.Ability.Description();
		}
	}

	private void HideCreatureInfo()
	{
		gameObject.SetActive(false);
	}

	// Functions for selected creature

	public void UseAbility()
	{
		UXManager.State.Selector.actionMarkers.ToggleAbility();
	}

	public void DestroyCreature()
	{
		UXManager.State.Selector.DestroySelectedCreature();
	}

}
