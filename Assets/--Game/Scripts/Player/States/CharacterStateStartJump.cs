using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStateStartJump : CharacterState
{
    [Title("States")]
    [SerializeField]
    CharacterState jumpState;

    [Title("Parameter")]
    [SerializeField]
    [SuffixLabel("en frames")]
    float crouchTime = 10f;

    float maxCrouchTime = 0f;
    float currentCrouchTime = 0f;

    public override void StartState(CharacterBase character, CharacterState oldState)
    {
        //character.Movement.animator.SetTrigger("Crouch");
        maxCrouchTime = crouchTime / 60f;
        currentCrouchTime = 0f;
    }

    public override void UpdateState(CharacterBase character)
    {
        if (GameManager.Instance.gamePaused) return;
        //currentCrouchTime += Time.deltaTime;
        character.Movement.Jump();
        character.Movement.animator.SetTrigger("Jumping");
        character.SetState(jumpState);
        //if (currentCrouchTime >= maxCrouchTime)
        //{
        //}
    }

    public override void LateUpdateState(CharacterBase character)
    {

    }

    public override void EndState(CharacterBase character, CharacterState newState)
    {

    }
}