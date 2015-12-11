using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Util
{
	// Defines extension methods that allow dictionaries to be treated as multisets
	public static class Multiset
	{
		// Adds up the two multisets into a third multiset.
		public static IDictionary<T, int> Add<T>(this IDictionary<T, int> first, IDictionary<T, int> second)
		{
			var result = new Dictionary<T, int>(first);
			foreach (var entry in second)
			{
				if (result.ContainsKey(entry.Key))
				{
					result[entry.Key] += entry.Value;
				}
				else
				{
					result[entry.Key] = entry.Value;
				}
			}
			return result;
		}

		// Check if one multiset contains another
		public static bool Contains<T>(this IDictionary<T, int> superset, IDictionary<T, int> subset)
		{
			return subset.All(entry => superset.ContainsKey(entry.Key) && superset[entry.Key] >= entry.Value);
		}
	}

	// Defines extension methods related to coordinates
	public static class Coordinates
	{
		public static Coordinate Coordinate(this MonoBehaviour script)
		{
			return GameManager.gm.ToGridCoordinate(script.gameObject.transform.position);
		}
	}
}
