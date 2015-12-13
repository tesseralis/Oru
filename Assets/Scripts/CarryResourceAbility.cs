using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Util;

public class CarryResourceAbility : MonoBehaviour, IAbility
{
	public int capacity;
	public ResourceCount[] carrying;

	// Convert to a multiset
	// TODO refactor with the same methods in ResourcePile
	private IDictionary<ResourceType, int> Carrying
	{
		get
		{
			return carrying.Aggregate(Multiset.Empty<ResourceType>(),
				(ms, resource) => ms.MultisetAdd(resource.type, resource.count));
		}
		set
		{
			carrying = value.Select(resource => new ResourceCount(resource.Key, resource.Value)).ToArray();
		}
	}

	public void Use(Coordinate target)
	{
		var resources = GameManager.Resources;

		// If we're not carrying anything, pick up things
		if (Carrying.IsEmpty())
		{
			var newCarry = Multiset.Empty<ResourceType>();
			var resourcePile = resources[target];
			var remainingCapacity = capacity;
			foreach (var resource in resources[target])
			{
				if (resource.Value >= remainingCapacity)
				{
					// We've reached the remaining capacity, so finalize our changes and break
					resources[target] = resourcePile.MultisetSubtract(resource.Key, remainingCapacity);
					Carrying = newCarry.MultisetAdd(resource.Key, remainingCapacity);
					return;
				}
				else
				{
					// Otherwise, accumulate that amount
					resourcePile = resourcePile.MultisetSubtract(resource.Key, resource.Value);
					newCarry = newCarry.MultisetAdd(resource.Key, resource.Value);
					remainingCapacity -= resource.Value;
				}
			}
			// TODO I know there's a more elegant way to phrase this
			resources[target] = resourcePile;
			Carrying = newCarry;
		}
		else
		{
			// Otherwise, put down what we're carrying right now
			// Put down what we're carrying on the coordinate
			resources[target] = resources[target].MultisetAdd(Carrying);
			Carrying = Multiset.Empty<ResourceType>();

		}
	}
}
