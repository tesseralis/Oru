using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

// Pop-up that shows what recipes we're trying to create right now.
public class RecipeInfo : MonoBehaviour
{
	public Text nameDisplay;
	public Text descriptionDisplay;

	public void DisplayRecipeInfo(CreatureType creatureType)
	{
		gameObject.SetActive(true);
		nameDisplay.text = creatureType.ToString();
		var definition = CreatureDefinitions.ForType(creatureType);
		string ability = definition.Ability != null ? definition.Ability.Description() : "None";
		descriptionDisplay.text = string.Format("Allowed Terrain: {0}\nAbility: {1}",
			string.Join(", ", definition.AllowedTerrain.Select(t => t.ToString()).ToArray()),
			ability);
		var creatureDefinition = CreatureDefinitions.ForType(creatureType);
		GetComponentInChildren<ResourceList>().ShowResources(creatureDefinition.Recipe);
	}
}
