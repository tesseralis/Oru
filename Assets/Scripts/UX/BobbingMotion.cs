using UnityEngine;
using System.Collections;


// Script that causes objects to bob up and down over time.
public class BobbingMotion : MonoBehaviour
{

	public float magnitude = 0.1f;
	public float period = 3f;
	
	// Update is called once per frame
	void Update ()
	{
		var position = transform.position;
		var newY = Mathf.Cos(Time.timeSinceLevelLoad * period) * magnitude;
		transform.position = new Vector3(position.x, newY, position.z);
	}
}
