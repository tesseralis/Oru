using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour
{
	public RectTransform currentHealth;

	public void SetHealth(int health)
	{
		var ratio = (float) health / CreatureController.maxHealth;
		var width = gameObject.GetComponent<RectTransform>().rect.width;
		var offsetRight = (1.0f - ratio) * width;
		currentHealth.offsetMax = Vector2.left * offsetRight;
	}
}
