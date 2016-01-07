using UnityEngine;
using System;

public class FightAbility : MonoBehaviour, IAbility
{
	protected Creature creature;

	// The enemy creature to target
	public Creature target;

	public int attack;
	public int defense;

	public class Definition : IAbilityDefinition
	{
		public int Attack { get; set; }
		public int Defense { get; set; }

		public string Description()
		{
			return "Attack enemies";
		}

		public IAbility AddToCreature(Creature creature)
		{
			var ability = creature.gameObject.AddComponent<FightAbility>();
			ability.attack = Attack;
			ability.defense = Defense;
			return ability;
		}
	}

	void Awake()
	{
		creature = GetComponent<Creature>();
	}

	public void Use(Coordinate coordinate)
	{
		// FIXME Set the enemy target
		var enemy = LevelManager.Creatures[coordinate];
		if (enemy && enemy.Definition.IsEnemy)
		{
			target = enemy;
		}
	}

	public void Passive()
	{
		foreach (var neighbor in creature.Position.CardinalNeighbors())
		{
			var enemy = LevelManager.Creatures[neighbor];
			if (enemy && (enemy.Definition.IsEnemy != creature.Definition.IsEnemy))
			{
				Debug.Log("Spotted enemy " + enemy);
				// Attack the enemy
				if (enemy.GetComponent<FightAbility>())
				{
					enemy.health -= Math.Max(attack - enemy.GetComponent<FightAbility>().defense, 1);
				}
				else
				{
					enemy.health = -1;
				}
				// TODO Figure out a way for this not to rely on UX!!!
				var particles = UXManager.Particles;
				particles.CreateParticle(particles.particleOptions.attack, enemy.Position);
			}
			if (target != null)
			{
				// TODO you should be able to do this without using the ability
				creature.SetGoal(target.Position);
			}
		}
	}

	public string Description()
	{
		return "Attack";
	}
}
