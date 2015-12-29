using UnityEngine;
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
		// TODO generalize for all abilities!
		string ability;
		if (creature.GetComponent<ChangeTerrainAbility>() != null)
		{
			useAbilityButton.gameObject.SetActive(true);
			ability = string.Format("Pick up {0}\n\nPress <Space> to activate.",
				creature.GetComponent<ChangeTerrainAbility>().carryType);
		}
		else if (creature.GetComponent<CarryResourceAbility>() != null)
		{
			useAbilityButton.gameObject.SetActive(true);
			ability = string.Format("Pick up {0} resources\n\nPress <Space> to activate.",
				creature.GetComponent<CarryResourceAbility>().capacity);
		}
		else
		{
			useAbilityButton.gameObject.SetActive(false);
			ability = "None";
		}

		resourceList.ShowResources(creatureDefinition.Recipe);

		descriptionDisplay.text = string.Format("Allowed Terrain: {0}\nAbility: {1}",
			string.Join(", ", creatureDefinition.AllowedTerrain.Select(t => t.ToString()).ToArray()),
			ability);
		DisplayAbilityText(creature);
	}

	private void DisplayAbilityText(Creature creature)
	{
		var text = useAbilityButton.GetComponentInChildren<Text>();
		if (creature.GetComponent<ChangeTerrainAbility>() != null)
		{
			var changeTerrain = creature.GetComponent<ChangeTerrainAbility>();
			if (changeTerrain.IsCarrying)
			{
				text.text = "Put down " + changeTerrain.carryType;
			}
			else
			{
				text.text = "Pick up " + changeTerrain.carryType;
			}
		}
		else if (creature.GetComponent<CarryResourceAbility>() != null)
		{
			var carryResource = creature.GetComponent<CarryResourceAbility>();
			if (carryResource.Carrying.IsEmpty())
			{
				text.text = "Pick up " + carryResource.capacity;
			}
			else
			{
				text.text = "Put down " + carryResource.capacity;
			}
		}
	}

	private void HideCreatureInfo()
	{
		gameObject.SetActive(false);
	}

}
