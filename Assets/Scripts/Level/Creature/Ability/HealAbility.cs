using UnityEngine;
using System;

public class HealAbility : MonoBehaviour, IAbility
{
	protected Creature creature;

	public int healPower;

	public class Definition : IAbilityDefinition
	{
		public int HealPower { get; set; }

		public string Description()
		{
			return "Heal allied creatures";
		}

		// TODO these functions look like they can be consolidated
		public IAbility AddToCreature(Creature creature)
		{
			var ability = creature.gameObject.AddComponent<HealAbility>();
			ability.healPower = HealPower;
			return ability;
		}
	}

	void Awake()
	{
		creature = GetComponentInChildren<Creature>();
	}

	public string Description()
	{
		return "Heal";
	}

	public void Use(Coordinate coordinate)
	{
		// TODO Use the same creature setting as the fight ability
	}

	public bool CanUse(Coordinate coordinate)
	{
		// TODO use the same method as the fight ability to target allied creatures
		return true;
	}

	public void Passive()
	{
		foreach (var neighbor in creature.Position.CardinalNeighbors())
		{
			var ally = LevelManager.Creatures[neighbor];
			if (ally && (ally.Definition.IsEnemy == creature.Definition.IsEnemy))
			{
				// Heal allies
				ally.health = Math.Min(ally.health + healPower, ResourceCollection.maxHealth);
			}
		}
	}
}