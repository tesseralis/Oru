using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour
{
	void Start()
	{
		LevelManager.Creatures.CreatureStepped += AnimateCreature;
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
