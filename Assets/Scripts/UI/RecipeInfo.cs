using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

// Pop-up that shows what recipes we're trying to create right now.
public class RecipeInfo : MonoBehaviour
{

	public Text nameDisplay;


	public void DisplayRecipeInfo(CreatureType creatureType)
	{
		gameObject.SetActive(true);
		nameDisplay.text = creatureType.ToString();
		var creatureDefinition = Creatures.ForType(creatureType);
		GetComponentInChildren<ResourceList>().SetContents(creatureDefinition.Recipe);
	}
}
