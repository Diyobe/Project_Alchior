using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateIdle : CharacterState
{
	[Title("States")]
	[SerializeField]
	CharacterState dashState;
	[SerializeField]
	CharacterState aerialState;
	[SerializeField]
	CharacterState wallRunState;
	[SerializeField]
	CharacterState jumpStartState;
	[SerializeField]
	CharacterState turnAroundState;


	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Movement.animator.SetBool("IsGrounded", true);
	}

	public override void UpdateState(CharacterBase character)
	{
		if (GameManager.Instance.gamePaused) return;

		character.Movement.HandleGravity();
		character.Movement.HandleMovement(character.inputPlayer.GetAxis("MoveX"), character.inputPlayer.GetAxis("MoveZ"));

		Vector3 newVector;
		newVector.x = character.Movement.SpeedX;
		newVector.y = 0;
		newVector.z = character.Movement.SpeedZ;

		float currentSpeed = Mathf.Clamp(newVector.magnitude * 2, 0, character.Movement.runningSpeed) / character.Movement.runningSpeed;
		character.Movement.animator.SetFloat("IdleMovement", currentSpeed);

        if (character.inputPlayer.GetButtonDown("Interact"))
        {
			character.actions.Interact();
        }

        if (character.inputPlayer.GetButtonDown("Jump"))
        {
			character.canLand = false;
			character.Movement.Jump();
			character.SetState(jumpStartState);
		}

		if (!character.Rigidbody.IsGrounded())
		{
			character.SetState(aerialState);
		}
	}
	
	public override void LateUpdateState(CharacterBase character)
	{

	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}
}