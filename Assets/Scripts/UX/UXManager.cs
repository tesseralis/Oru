using UnityEngine;
using System.Collections;

public class UXManager : MonoBehaviour
{

	public AudioController audioController;
	public InputController inputController;
	public ParticleController particleController;

	public static UXManager ux;

	public static AudioController Audio { get { return ux.audioController; } }
	public static InputController Input { get { return ux.inputController; } }
	public static ParticleController Particles { get { return ux.particleController; } }

	void Awake()
	{
		if (ux == null) { ux = this; }

		if (!audioController) { audioController = GetComponentInChildren<AudioController>(); }
		if (!inputController) { inputController = GetComponentInChildren<InputController>(); }
		if (!particleController) { particleController = GetComponent<ParticleController>(); }
	}

}
