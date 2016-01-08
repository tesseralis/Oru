﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using Util;

public class CreatureInfo : MonoBehaviour
{

	public Text nameDisplay;
	public Text descriptionDisplay;
	public ResourceList resourceList;
	public DelegateButton useAbilityButton;
	public DelegateButton destroyCreatureButton;

	public void Start()
	{
		UXManager.State.Selector.Deselected += HideCreatureInfo;
		UXManager.State.Selector.Selected += DisplayCreatureInfo;

		// Add handlers for using ability
		useAbilityButton.Click += UXManager.State.Selector.actionMarkers.ToggleAbility;

		// Add handlers for destroying the creature
		destroyCreatureButton.Click += UXManager.State.Selector.DestroySelectedCreature;

		UXManager.State.Selector.AbilityUsed += () =>
			DisplayAbilityText(UXManager.State.Selector.SelectedCreature);

		// Finally, hide ourselves until a creature is selected
		HideCreatureInfo();
	}

	private void DisplayCreatureInfo(Creature creature)
	{
		gameObject.SetActive(true);
		nameDisplay.text = creature.creatureType.ToString();
		var creatureDefinition = CreatureDefinitions.ForType(creature.creatureType);
		useAbilityButton.gameObject.SetActive(creature.HasAbility());
		string ability = creature.HasAbility() ? creature.Definition.Ability.Description() : "None";
		descriptionDisplay.text = string.Format("Allowed Terrain: {0}\nAbility: {1}\n\nHealth: {2}",
			string.Join(", ", creatureDefinition.AllowedTerrain.Select(t => t.ToString()).ToArray()),
			ability,
			creature.health);

		resourceList.ShowResources(creatureDefinition.Recipe);

		DisplayAbilityText(creature);
		var isEnemy = creature.Definition.IsEnemy;
		useAbilityButton.gameObject.SetActive(!isEnemy);
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

}
