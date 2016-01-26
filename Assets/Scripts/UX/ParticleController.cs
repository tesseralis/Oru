using UnityEngine;
using System;
using System.Collections;
using Util;

public class ParticleController : MonoBehaviour
{
	public ParticleEffectOptions particleOptions;

	// Use this for initialization
	void Start () {
		// Add particles on certain effects
		LevelManager.Creatures.CreatureCreated += (x, pos) => CreateParticle(particleOptions.createCreature, pos);
		LevelManager.Creatures.CreatureDestroyed += (c, pos) => CreateParticle(particleOptions.destroyCreature, pos);
		UXManager.State.Selector.GoalSet += (creature, pos) => CreateParticle(particleOptions.setCreatureGoal, pos);
		LevelManager.Creatures.CreatureStepped += LowHealth;
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
}

[Serializable]
public class ParticleEffectOptions
{
	public GameObject createCreature;
	public GameObject destroyCreature;
	public GameObject setCreatureGoal;
	public GameObject lowHealth;
	public GameObject attack;
	public GameObject heal;
}
