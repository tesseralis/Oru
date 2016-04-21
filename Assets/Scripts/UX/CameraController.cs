using UnityEngine;
using System;
using System.Collections;

public class CameraController : MonoBehaviour
{

	public InputOptions inputOptions;

	// Initialize our camera scale
	public InitScaleOptions xScale = new InitScaleOptions(31.7f, 0.14f);
	public InitScaleOptions yScale = new InitScaleOptions(26.94f, -0.73f);
	public InitScaleOptions zScale = new InitScaleOptions(-33.76f, 1.86f);
	public InitScaleOptions sizeScale = new InitScaleOptions(-0.027f, 0.86f);



	void Awake()
	{
		LevelManager.LevelLoaded += SetCamera;
	}

	void SetCamera(LevelManager level)
	{
		// Set the camera's position based on level size
		var size = (LevelManager.Terrain.Width + LevelManager.Terrain.Height) / 2;

		Camera.main.orthographicSize = sizeScale.Calculate(size);
		Camera.main.transform.position = new Vector3(xScale.Calculate(size), yScale.Calculate(size), zScale.Calculate(size));
	}

	void Update()
	{
		DoPan(UXManager.Input.PanDirection());
		DoZoom(UXManager.Input.ZoomAmount());
	}

	// Pan our camera in the specified direction
	private void DoPan(Vector2 direction)
	{
		transform.Translate(new Vector3(direction.x, direction.y, direction.x) * inputOptions.translateFactor);
	}

	private void DoZoom(float amount)
	{
		var newZoom = Camera.main.orthographicSize + amount * inputOptions.zoomFactor;
		Camera.main.orthographicSize = Mathf.Clamp(newZoom, inputOptions.minZoom, inputOptions.maxZoom);
	}

}

[Serializable]
public class InputOptions
{
	public float translateFactor = 0.25f;
	public float zoomFactor = 1.0f;
	public float minZoom = 1.0f;
	public float maxZoom = 10.0f;
}

// Options for scaling the camera on initial level load
[Serializable]
public class InitScaleOptions
{
	public float intersect;
	public float slope;

	public InitScaleOptions(float intersect, float slope)
	{
		this.intersect = intersect;
		this.slope = slope;
	}

	public float Calculate(float size)
	{
		return intersect + slope * size;
	}
}