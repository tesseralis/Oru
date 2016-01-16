using UnityEngine;
using System.Collections;


// Script that causes objects to bob up and down over time.
public class BobbingMotion : MonoBehaviour
{

	public float magnitude = 0.0025f;
	public float period = 3f;
	
	// Update is called once per frame
	void Update ()
	{
		gameObject.transform.Translate(Vector3.up * Mathf.Cos(Time.timeSinceLevelLoad * period) * magnitude);
	}
}
