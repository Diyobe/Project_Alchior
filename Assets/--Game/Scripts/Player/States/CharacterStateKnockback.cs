using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateKnockback : CharacterState
{

	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Movement.animator.SetTrigger("Knockback");
	}

	public override void UpdateState(CharacterBase character)
	{
		character.Knockback.UpdateKnockback();
		if (character.Knockback.KnockbackDuration <= 0)
		{
			character.ResetToIdle();
		}
	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}
}