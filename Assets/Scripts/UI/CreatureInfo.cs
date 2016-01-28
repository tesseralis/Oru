using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Util;

public class CreatureInfo : MonoBehaviour
{
	public GameObject panel;
	public Text nameDisplay;
	public Text blueprintLabel;
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
		// If a blueprint is highlighed and is not the same as the current creating creature (if any),
		// show that blueprint's info
		if (highlightedBlueprint.HasValue
			&& (!creatingCreature.HasValue || highlightedBlueprint.Value != creatingCreature.Value))
		{
			blueprintLabel.text = "Blueprint";
			DisplayRecipeInfo(highlightedBlueprint.Value);
		}
		else if (creatingCreature.HasValue)
		{
			blueprintLabel.text = "Creating";
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
		blueprintLabel.gameObject.SetActive(true);

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
		blueprintLabel.gameObject.SetActive(false);

		// Display creature data
		useAbilityButton.gameObject.SetActive(!creature.Definition.IsEnemy && creature.HasAbility());

		DisplayAbilityButtonText(creature);
		destroyCreatureButton.gameObject.SetActive(!creature.Definition.IsEnemy);
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
			text += "\n\nAbility: " + definition.Ability.Description();
		}
		// TODO makes this more typesafe?
		if (definition.Ability is FightAbility.Definition)
		{
			var ability = (FightAbility.Definition)definition.Ability;
			text += "\n\nAttack: " + ability.Attack;
			text += "\nDefense: " + ability.Defense;
		}
		descriptionDisplay.text = text;

		resourceList.ShowResources(definition.RecipeWithEnergy());
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
