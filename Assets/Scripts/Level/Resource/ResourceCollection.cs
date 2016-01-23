using System;
using System.Collections.Generic;
using System.Linq;
using Util;

[Serializable]
public class ResourceCollection
{
//	public const int maxHealth = 50;

	// Paper resources and full energy blocks
	public ResourceCount[] paper = new ResourceCount[0];
	// Used up energy blocks
	public int[] energyBlocks = new int[0];

	public IList<int> EnergyBlocks
	{
		get
		{
			var fullEnergies = paper.Where(x => x.type == ResourceType.Energy).Sum(x => x.count);
			return energyBlocks.Concat(Enumerable.Repeat(CreatureController.maxHealth, fullEnergies)).ToList();
		}
		private set { energyBlocks = value.ToArray(); }
	}
	public IDictionary<ResourceType, int> Paper
	{
		get
		{
			return paper.Where(x => x.type != ResourceType.Energy).Aggregate(Multiset.Empty<ResourceType>(),
				(ms, resource) => ms.MultisetAdd(resource.type, resource.count));
		}
		private set { paper = value.Select(resource => new ResourceCount(resource.Key, resource.Value)).ToArray(); }
	}

	public bool IsEmpty()
	{
		return EnergyBlocks.Count == 0 && Paper.IsEmpty();
	}

	// Return a flat multiset of this collection, losing energy block health info
	public IDictionary<ResourceType, int> ToMultiset()
	{
		return Paper.MultisetAdd(ResourceType.Energy, EnergyBlocks.Count);
	}

	public static ResourceCollection FromMultiset(IDictionary<ResourceType, int> ms)
	{
		return new ResourceCollection
		{
			EnergyBlocks = new List<int>(),
			Paper = ms
		};
	}

	public ResourceCollection WithPaper(IDictionary<ResourceType, int> paper)
	{
		return new ResourceCollection
		{
			EnergyBlocks = this.EnergyBlocks,
			Paper = paper
		};
	}

	// Return a collection with a single instance of the given energy block removed
	public ResourceCollection Remove(int energy)
	{
		var newBlocks = new List<int>(EnergyBlocks);
		newBlocks.Remove(energy);
		return new ResourceCollection
		{
			EnergyBlocks = newBlocks,
			Paper = this.Paper
		};
	}

	public ResourceCollection Add(int energy)
	{
		return new ResourceCollection
		{
			EnergyBlocks = this.EnergyBlocks.Concat(new List<int>() { energy }).ToList(),
			Paper = this.Paper
		};
	}

	// Take n items arbitrarily from this collection and leave the remaining
	public ResourceCollection Take(int n, out ResourceCollection taken)
	{
		if (EnergyBlocks.Count >= n)
		{
			taken = new ResourceCollection
			{
				EnergyBlocks = this.EnergyBlocks.Take(n).ToList(),
				Paper = Multiset.Empty<ResourceType>()
			};
			return new ResourceCollection
			{
				EnergyBlocks = this.EnergyBlocks.Skip(n).ToList(),
				Paper = this.Paper
			};
		}
		n -= EnergyBlocks.Count;
		var newPaper = Multiset.Empty<ResourceType>();
		var remainder = Paper;
		foreach (ResourceType type in Paper.Keys)
		{
			var count = Paper[type];
			if (count >= n)
			{
				taken = new ResourceCollection
				{
					EnergyBlocks = this.EnergyBlocks,
					Paper = newPaper.MultisetAdd(type, n)
				};
				return new ResourceCollection
				{
					EnergyBlocks = new List<int>(),
					Paper = remainder.MultisetSubtract(type, n)
				};
			}
			else
			{
				newPaper = newPaper.MultisetAdd(type, count);
				remainder = remainder.MultisetSubtract(type, count);
				n -= count;
			}
		}
		// If we go through all the resources, there is nothing left
		taken = this;
		return Empty();
	}

	public static ResourceCollection Empty()
	{
		return new ResourceCollection
		{
			EnergyBlocks = Enumerable.Empty<int>().ToList(),
			Paper = Multiset.Empty<ResourceType>()
		};
	}

	public static ResourceCollection operator +(ResourceCollection c1, ResourceCollection c2)
	{
		return new ResourceCollection
		{
			EnergyBlocks = c1.EnergyBlocks.Concat(c2.EnergyBlocks).ToList(),
			Paper = c1.Paper.MultisetAdd(c2.Paper)
		};
	}
}

[Serializable]
public class ResourceCount
{
	public ResourceCount(ResourceType _type, int _count)
	{
		type = _type;
		count = _count;
	}

	public ResourceType type;
	public int count;
}
