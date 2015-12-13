using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class RecipeListPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach(var button in GetComponentsInChildren<Button>(true))
		{
			button.gameObject.SetActive(false);
		}
	}
}
