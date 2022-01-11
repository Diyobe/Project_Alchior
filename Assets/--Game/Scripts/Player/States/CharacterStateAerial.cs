using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateAerial : CharacterState
{
    [Title("States")]
    [SerializeField]
    CharacterState landingState;

    [Title("Parameter - Actions")]
    [SerializeField]
    CharacterMoveset moveset;

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        //character.Movement.animator.SetBool("IsGrounded", false);
        character.Movement.ResetAnimator();

        character.Movement.animator.ResetTrigger("Fall");
        character.Movement.animator.SetTrigger("Fall");
        StartCoroutine(StartCanLand(character));
    }

    public override void UpdateState(CharacterBase character)
    {
        if (GameManager.Instance.gamePaused) return;

        if (moveset.ActionAttackAerial(character))
        {
            //return;
        }
        character.Movement.AirControl(character.inputPlayer.GetAxis("MoveX"), character.inputPlayer.GetAxis("MoveZ"));
    }

    public override void LateUpdateState(CharacterBase character)
    {
        if (GameManager.Instance.gamePaused) return;

        if (character.Rigidbody.IsGrounded() && character.canLand)
        {
            character.Movement.isJumping = false;
            moveset.aerialAttack = false;
            character.SetState(landingState);
        }
        character.Movement.HandleGravity();
    }

    public override void EndState(CharacterBase character, CharacterState newState)
    {

    }

    IEnumerator StartCanLand(CharacterBase character)
    {
        yield return new WaitForSeconds(0.01f);
        character.canLand = true;
    }
}