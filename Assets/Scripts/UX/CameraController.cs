using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	public float translateFactor = 0.25f;
	public float zoomFactor = 1.0f;
	public float minZoom = 1.0f;
	public float maxZoom = 10.0f;

	void Update()
	{
		DoPan(UXManager.Input.PanDirection());
		DoZoom(UXManager.Input.ZoomAmount());
	}

	// Pan our camera in the specified direction
	private void DoPan(Vector2 direction)
	{
		transform.Translate(new Vector3(direction.x, direction.y, direction.x) * translateFactor);
	}

	private void DoZoom(float amount)
	{
		var newZoom = Camera.main.orthographicSize + amount * zoomFactor;
		Camera.main.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
	}

}
