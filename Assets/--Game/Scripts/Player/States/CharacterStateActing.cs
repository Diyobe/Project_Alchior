using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateActing : CharacterState
{
    [Header("Parameter - Actions")]
    [SerializeField] CharacterMoveset characterMoveset;
    bool isAerial = false;

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        if (oldState is CharacterStateAerial)
        {
            isAerial = true;
        }
        else if (oldState is CharacterStateIdle)
        {
            isAerial = false;
        }
    }

    public override void UpdateState(CharacterBase character)
    {
        if (isAerial)
        {
            if (characterMoveset.ActionAttackAerial(character)) return;
        }
        else
        {
            if (characterMoveset.ActionAttackGrounded(character)) return;
        }

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