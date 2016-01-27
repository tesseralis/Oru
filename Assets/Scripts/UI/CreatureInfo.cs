using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Util;

public class CreatureInfo : MonoBehaviour
{

	public Text nameDisplay;
	public Healthbar healthbar;
	public Text descriptionDisplay;
	public ResourceList resourceList;
	public Button useAbilityButton;
	public Button destroyCreatureButton;

	public void Start()
	{
		UXManager.State.Selector.Deselected += HideCreatureInfo;
		UXManager.State.Selector.Selected += DisplayCreatureInfo;

		LevelManager.Creatures.CreaturesUpdated += (obj) => 
		{
			if (UXManager.State.Selector.SelectedCreature)
			{
				DisplayCreatureInfo(UXManager.State.Selector.SelectedCreature);
			}
		};

		UXManager.State.Selector.AbilityUsed += () =>
			DisplayAbilityText(UXManager.State.Selector.SelectedCreature);

		// Finally, hide ourselves until a creature is selected
		HideCreatureInfo();
	}

	private void DisplayCreatureInfo(Creature creature)
	{
		gameObject.SetActive(true);

		// Display the creature's name
		nameDisplay.text = creature.creatureType.ToString();

		// Display creature health
		healthbar.SetHealth(creature.health);

		// Display creature data
		var creatureDefinition = CreatureDefinition.ForType(creature.creatureType);
		useAbilityButton.gameObject.SetActive(!creature.Definition.IsEnemy && creature.HasAbility());
		// TODO refactor with same method in "Recipe Info"
		var text = "Allowed Terrain: " + string.Join(", ", creatureDefinition.AllowedTerrain.Select(t => t.ToString()).ToArray());
		if (creature.HasAbility())
		{
			text += "\nAbility: " + creature.Definition.Ability.Description();
		}
		descriptionDisplay.text = text;

		// Display the creature's resources
		resourceList.ShowResources(creatureDefinition.RecipeWithEnergy());

		DisplayAbilityText(creature);
		var isEnemy = creature.Definition.IsEnemy;
		destroyCreatureButton.gameObject.SetActive(!isEnemy);
	}

	private void DisplayAbilityText(Creature creature)
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

	public void UseAbility()
	{
		UXManager.State.Selector.actionMarkers.ToggleAbility();
	}

	public void DestroyCreature()
	{
		UXManager.State.Selector.DestroySelectedCreature();
	}

}
