using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateLanding : CharacterState
{
	[Title("States")]
	[SerializeField]
	CharacterState idleState;

	[Title("Parameter")]
	[SerializeField]
	[SuffixLabel("en frames")]
	float landingTime = 10f;

	//[SerializeField]
	//CharacterEvasiveMoveset evasiveMoveset;

	[Title("Particle")] // ? d?gager un jour
	[SerializeField]
	ParticleSystem landParticleSystem;

	float maxLandingTime = 0f;
	float currentLandingTime = 0f;

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Movement.isJumping = false;
		character.canLand = false;
		character.Movement.animator.SetTrigger("Landing");
        maxLandingTime = landingTime / 60f;
		currentLandingTime = 0f;

		//ParticleSystem particle = landParticleSystem ? Instantiate(landParticleSystem, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
		//Destroy(particle.gameObject, 0.5f);

		//evasiveMoveset.ResetDodge();
	}

	public override void UpdateState(CharacterBase character)
	{
		if (GameManager.Instance.gamePaused) return;
		character.Movement.SpeedX *= 0.9f;
		currentLandingTime += Time.deltaTime;

		if (currentLandingTime >= maxLandingTime)
		{
			character.SetState(idleState);
		}
	}
	
	//public override void LateUpdateState(CharacterBase character)
	//{

	//}

	//public override void EndState(CharacterBase character, CharacterState newState)
	//{

	//}
}