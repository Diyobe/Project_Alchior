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

	[Title("Parameter - Actions")]
	[SerializeField]
	CharacterMoveset moveset;


	public override void StartState(CharacterBase character, CharacterState oldState)
	{
		character.Movement.ResetAnimator();
		character.Movement.animator.SetTrigger("Idle");
		//character.Movement.animator.SetBool("IsGrounded", true);
	}

	public override void UpdateState(CharacterBase character)
	{
		if (GameManager.Instance.gamePaused) return;
		character.Movement.HandleGravity();
		character.Movement.HandleMovement(character.inputPlayer.GetAxis("MoveX"), character.inputPlayer.GetAxis("MoveZ"));



        

        if (character.inputPlayer.GetButtonDown("Jump"))
        {
			character.SetState(jumpStartState);
			character.Movement.Jump();
			character.canLand = false;
		}
		else if (moveset.ActionAttackGrounded(character))
		{
			return;
		}
		else if (character.inputPlayer.GetButtonDown("ItemInteract") && !GameManager.Instance.gamePaused)
		{
			character.actions.Interact();
		}


	}
	
	public override void LateUpdateState(CharacterBase character)
	{
		Vector3 newVector;
		newVector.x = character.Movement.SpeedX;
		newVector.y = 0;
		newVector.z = character.Movement.SpeedZ;
		float currentSpeed = Mathf.Clamp(newVector.magnitude * 2, 0, character.Movement.runningSpeed) / character.Movement.runningSpeed;
		character.Movement.animator.SetFloat("IdleMovement", currentSpeed);
		if (!character.Rigidbody.IsGrounded())
		{
			character.SetState(aerialState);
		}
	}

	public override void EndState(CharacterBase character, CharacterState newState)
	{

	}
}