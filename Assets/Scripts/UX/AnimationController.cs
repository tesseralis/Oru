using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
	void Start()
	{
		LevelManager.Creatures.CreatureStepped += AnimateCreature;
		UXManager.State.Selector.CreatureError += AnimateError;
	}

	private void AnimateError(Creature creature)
	{
		if (!creature.Definition.IsEnemy)
		{
			if (creature.GetComponentInChildren<Animator>())
			{
				creature.GetComponentInChildren<Animator>().SetTrigger("Error");
			}
		}
	}

	private void AnimateCreature(Creature creature)
	{
		if (creature.GetComponentInChildren<Animator>())
		{
			bool isMoving = creature.Position != creature.Goal;
			creature.GetComponentInChildren<Animator>().SetBool("Moving", isMoving);
		}
	}
}
