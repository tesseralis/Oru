using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class CreatureInfo : MonoBehaviour
{

	public Text nameDisplay;

	public Text descriptionDisplay;

	public void DisplayCreatureInfo(Creature creature)
	{
		gameObject.SetActive(true);
		nameDisplay.text = creature.creatureType.ToString();
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

		descriptionDisplay.text = string.Format("Allowed Terrain: {0}\nAbility: {1}",
			string.Join(", ", creatureDefinition.AllowedTerrain.Select(t => t.ToString()).ToArray()),
			ability);
	}

}
