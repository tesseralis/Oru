using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Util
{

	// Defines extension methods related to coordinates
	// TODO I don't think an extension method is the right thing to do here. It should be some sort of base class.
	public static class MonoBehaviorUtils
	{
		public static Coordinate Coordinate(this MonoBehaviour script)
		{
			return GameManager.gm.ToGridCoordinate(script.gameObject.transform.position);
		}

		/// <summary>
		/// Add a child object to the script with the given component, using the given prefab.
		/// </summary>
		/// <returns>The component belonging to the child that was added.</returns>
		/// <typeparam name="T">The type of component to add.</typeparam>
		public static T AddChildWithComponent<T>(this MonoBehaviour script, GameObject prefab, Coordinate coordinate) where T : Component
		{
			GameObject newObject = GameObject.Instantiate(prefab);
			GameManager.gm.SetPosition(newObject, coordinate);
			newObject.transform.SetParent(script.transform);

			if (newObject.GetComponent<T>() == null)
			{
				newObject.AddComponent<T>();
			}
			return newObject.GetComponent<T>();
		}
	}
}
