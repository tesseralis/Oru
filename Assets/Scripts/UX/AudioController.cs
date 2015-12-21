using UnityEngine;
using System;
using System.Collections;

public class AudioController : MonoBehaviour {
	
	public SoundEffectOptions soundOptions;

	// Use this for initialization
	void Start ()
	{
		// Play sounds when the creature takes actions
		LevelManager.Creatures.CreatureCreated += (x, y) => PlaySound(soundOptions.createCreature);
		LevelManager.Creatures.CreatureDestroyed += (pos) => PlaySound(soundOptions.destroyCreature);
		LevelManager.Recipes.RecipesUpdated += (obj) => PlaySound(soundOptions.pickupRecipe);
		UIManager.ui.entitySelector.Selected += x => PlaySound(soundOptions.selectCreature);
		UIManager.ui.entitySelector.GoalSet += (x, y) => PlaySound(soundOptions.setCreatureGoal);
		UIManager.ui.entitySelector.actionMarkers.AbilityUsed += () => PlaySound(soundOptions.useAbility);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Play the given sound
	void PlaySound(AudioClip clip)
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
	// TODO separate audio for different abilities
	public AudioClip useAbility;
}
