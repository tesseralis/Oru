using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Healthbar : MonoBehaviour
{
	public RectTransform currentHealth;
	public Color healthColor = Color.green;
	public Color lowHealthColor = Color.red;

	public void SetHealth(int health)
	{
		// Set the current health to be a ratio of the full health
		var ratio = (float) health / CreatureController.maxHealth;
		var width = gameObject.GetComponent<RectTransform>().rect.width;
		var offsetRight = (1.0f - ratio) * width;
		currentHealth.offsetMax = Vector2.left * offsetRight;

		var image = currentHealth.GetComponent<Image>();
		// Change the color of the health bar
		image.color = health <= CreatureController.lowHealth ? lowHealthColor : healthColor;
	}
}
