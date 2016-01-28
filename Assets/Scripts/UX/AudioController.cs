using UnityEngine;
using System;
using System.Collections;

public class AudioController : MonoBehaviour {
	
	public SoundEffectOptions soundOptions;

	void Awake()
	{
		LevelManager.LevelLoaded += AddHooks;
	}


	// Use this for initialization
	void AddHooks (LevelManager level)
	{
		// Play sounds when the creature takes actions
		LevelManager.Creatures.CreatureCreated += (x, y) => PlaySound(soundOptions.createCreature);
		LevelManager.Creatures.CreatureDestroyed += (c, pos) => PlaySound(soundOptions.destroyCreature);
		LevelManager.Recipes.RecipesUpdated += (obj) => PlaySound(soundOptions.pickupRecipe);
		LevelManager.Goals.LevelCompleted += () => PlaySound(soundOptions.levelComplete);
		UXManager.State.Selector.Selected += x => PlaySound(soundOptions.selectCreature);
		UXManager.State.Selector.GoalSet += (x, y) => PlaySound(soundOptions.setCreatureGoal);
		UXManager.State.Selector.AbilityUsed += () => PlaySound(soundOptions.useAbility);
		UXManager.State.Selector.CreatureError += x => PlaySound(soundOptions.error);
		UXManager.State.Creator.CreationError += () => PlaySound(soundOptions.error);
	}

	// Play the given sound
	public void PlaySound(AudioClip clip)
	{
		if (clip)
		{
			// TODO perhaps refactor this?
			Camera.main.GetComponent<AudioSource>().PlayOneShot(clip);
		}
	}

}

[Serializable]
public class SoundEffectOptions
{
	public AudioClip destroyCreature;
	public AudioClip createCreature;
	public AudioClip selectCreature;
	public AudioClip pickupRecipe;
	public AudioClip setCreatureGoal;
	public AudioClip error;
	// TODO separate audio for different abilities
	public AudioClip useAbility;
	public AudioClip attack;
	public AudioClip levelComplete;
}
