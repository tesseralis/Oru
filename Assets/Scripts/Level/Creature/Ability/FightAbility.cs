using UnityEngine;
using System;

public class FightAbility : MonoBehaviour, IAbility
{
	private const int baseDamage = 2;

	protected Creature creature;

	// The enemy creature to target
	public Creature target;

	public BattlePower attack;
	public BattlePower defense;

	public class Definition : IAbilityDefinition
	{
		public BattlePower Attack { get; set; }
		public BattlePower Defense { get; set; }

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
		var enemy = LevelManager.Creatures[coordinate];
		if (enemy && enemy.Definition.IsEnemy)
		{
			target = enemy;
		}
	}

	public bool CanUse(Coordinate coordinate)
	{
		var otherCreature = LevelManager.Creatures[coordinate];
		// TODO make this stick to the enemy creature
		return (otherCreature != null) && otherCreature.Definition.IsEnemy != creature.Definition.IsEnemy;
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
					enemy.health -= CalculateDamage(enemy.GetComponent<FightAbility>());
				}
				else
				{
					enemy.health = -1;
				}

				// Face the direction of the enemy
				creature.FaceDirection(enemy.Position - creature.Position);

				// TODO Figure out a way for this not to rely on UX!!!
				var particles = UXManager.Particles;
				particles.CreateParticle(particles.particleOptions.attack, enemy.Position);
				var audio = UXManager.Audio;
				audio.PlaySound(audio.soundOptions.attack);

				// Only attack one enemy at a time
				break;
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

	private int CalculateDamage(FightAbility enemy)
	{
		// Damage is calculated by multiplying by the base damage by the square of the difference in the creatures strengths
		// and dividing by the creature's speed (since faster creatures can attack more).
		// TODO a fast low speed creature won't be able to hit a high def enemy because of rounding
		var diff = attack - enemy.defense + 3;
		return (diff * diff * baseDamage) / (int)creature.Definition.Speed(creature);
	}
}
