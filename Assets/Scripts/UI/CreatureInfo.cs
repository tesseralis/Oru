using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class CreatureInfo : MonoBehaviour
{

	public Text nameDisplay;
	public Text descriptionDisplay;
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

		// Finally, hide ourselves until a creature is selected
		HideCreatureInfo();
	}

	private void DisplayCreatureInfo(Creature creature)
	{
		gameObject.SetActive(true);
		nameDisplay.text = creature.creatureType.ToString();
		var creatureDefinition = Creatures.ForType(creature.creatureType);
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

		descriptionDisplay.text = string.Format("Allowed Terrain: {0}\nAbility: {1}",
			string.Join(", ", creatureDefinition.AllowedTerrain.Select(t => t.ToString()).ToArray()),
			ability);
	}

	private void HideCreatureInfo()
	{
		gameObject.SetActive(false);
	}

}
