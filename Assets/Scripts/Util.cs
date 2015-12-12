using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Util
{

	// Defines extension methods related to coordinates
	// TODO I don't think an extension method is the right thing to do here. It should be some sort of base class.
	public static class Coordinates
	{
		public static Coordinate Coordinate(this MonoBehaviour script)
		{
			return GameManager.gm.ToGridCoordinate(script.gameObject.transform.position);
		}
	}
}
