using UnityEngine;
using System;
using System.Collections;
using Util;

public class ParticleController : MonoBehaviour
{
	public ParticleEffectOptions particleOptions;

	void Awake()
	{
		LevelManager.LevelLoaded += AddHooks;
	}

	// Use this for initialization
	void AddHooks (LevelManager level) {
		// Add particles on certain effects
		LevelManager.Creatures.CreatureCreated += (x, pos) => CreateParticle(particleOptions.createCreature, pos);
		LevelManager.Creatures.CreatureDestroyed += (c, pos) => CreateParticle(particleOptions.destroyCreature, pos);
		LevelManager.Creatures.CreatureStepped += LowHealth;
		LevelManager.Creatures.CreatureStepped += IsEnemy;
		UXManager.State.Selector.GoalSet += (creature, pos) => CreateParticle(particleOptions.setCreatureGoal, pos);
	}

	public void CreateParticle(GameObject particles, Coordinate coordinate)
	{
		if (particles)
		{
			gameObject.AddChild(particles, coordinate);
		}
	}

	// Apply a "smoky" particle effect when the creature's health is low
	private void LowHealth(Creature creature)
	{
		if (creature.health <= CreatureController.lowHealth)
		{
			creature.gameObject.AddChild(particleOptions.lowHealth);
		}
	}

	private void IsEnemy(Creature creature)
	{
		if (creature.Definition.IsEnemy)
		{
			creature.gameObject.AddChild(particleOptions.enemy);
		}
	}
}

[Serializable]
public class ParticleEffectOptions
{
	public GameObject createCreature;
	public GameObject destroyCreature;
	public GameObject setCreatureGoal;
	public GameObject enemy;
	public GameObject lowHealth;
	public GameObject attack;
	public GameObject heal;
}
