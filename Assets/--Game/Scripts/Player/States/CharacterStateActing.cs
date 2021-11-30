using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateActing : CharacterState
{
	[Header("Parameter - Actions")]
	[SerializeField] CharacterMoveset characterMoveset;

	//public override void StartState(CharacterBase character, CharacterState oldState)
	//{

	//}

	public override void UpdateState(CharacterBase character)
	{
		if (characterMoveset.ActionAttackGrounded(character)) return;

		if (character.Action.movementCancelable && (Mathf.Abs(character.inputPlayer.GetAxis("MoveX")) > .5f || Mathf.Abs(character.inputPlayer.GetAxis("MoveZ")) > .5f))
			character.Action.FinishAction();

	}
	
	//public override void LateUpdateState(CharacterBase character)
	//{

	//}

	//public override void EndState(CharacterBase character, CharacterState newState)
	//{

	//}
}