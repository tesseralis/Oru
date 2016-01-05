using UnityEngine;
using System.Collections;

public class Pan : MonoBehaviour
{

	public float translateFactor = 0.25f;

	void Update()
	{
		DoPan(UXManager.Input.PanDirection());
	}

	// Pan our camera in the specified direction
	private void DoPan(Vector2 direction)
	{
		transform.Translate(new Vector3(direction.x, direction.y, direction.x) * translateFactor);
	}

}
