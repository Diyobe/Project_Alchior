using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateAerial : CharacterState
{
    [Title("States")]
    [SerializeField]
    CharacterState landingState;

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        character.Movement.animator.SetBool("IsGrounded", false);
    }

    public override void UpdateState(CharacterBase character)
    {
        character.Movement.AirControl(character.inputPlayer.GetAxis("MoveX"), character.inputPlayer.GetAxis("MoveZ"));
    }

    public override void LateUpdateState(CharacterBase character)
    {
        if (character.Rigidbody.IsGrounded())
        {
            character.Movement.isJumping = false;
            character.SetState(landingState);
        }
        character.Movement.HandleGravity();
    }

    public override void EndState(CharacterBase character, CharacterState newState)
    {

    }
}