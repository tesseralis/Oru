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
		var definition = CreatureDefinition.ForType(creatureType);
		// TODO refactor with same method in "Recipe Info"
		var text = "Allowed Terrain: " + string.Join(", ", definition.AllowedTerrain.Select(t => t.ToString()).ToArray());
		if (definition.Ability != null)
		{
			text += "\nAbility: " + definition.Ability.Description();
		}
		if (definition.Ability is FightAbility.Definition)
		{
			var ability = (FightAbility.Definition)definition.Ability;
			text += "\nAttack: " + ability.Attack;
			text += "\nDefense: " + ability.Defense;
		}
		descriptionDisplay.text = text;
		var creatureDefinition = CreatureDefinition.ForType(creatureType);
		GetComponentInChildren<ResourceList>().ShowResources(creatureDefinition.RecipeWithEnergy());
	}

	public void HideRecipeInfo()
	{
		gameObject.SetActive(false);
	}
}
