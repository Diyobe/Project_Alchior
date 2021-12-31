using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterMoveset : MonoBehaviour
{
    [Title("Parameter - Ground Weak Attacks")]
    [SerializeField]
    AttackManager groundedWeakAttackFirstHit = null;
    [SerializeField]
    AttackManager launcher = null;

    [Title("Parameter - Ground Heavy Attacks")]
    [SerializeField]
    AttackManager groundedHeavyAttack = null;

    [Title("Parameter - Aerial Weak Attacks")]
    [SerializeField]
    AttackManager aerialWeakAttackFirstHit = null;

    [Title("Parameter - Aerial Heavy Attacks")]
    [SerializeField]
    AttackManager aerialHeavyAttack = null;

    [HideInInspector] public bool aerialAttack = false;

    [Title("States")]
    [SerializeField]
    CharacterState stateAction;

    public bool ActionAttackGrounded(CharacterBase character, bool canSpecial = true)
    {
        //Check des inputs pour savoir si on lance une attaque ou non
        if (character.inputPlayer.GetButtonDown("WeakAttack"))
            return ActionAttack(character, groundedWeakAttackFirstHit);

        if (character.inputPlayer.GetButtonDown("StrongAttack"))
            return ActionAttack(character, groundedHeavyAttack);
        return false;
    }

    public bool ActionAttackAerial(CharacterBase character, bool canSpecial = true)
    {
        //Check des inputs pour savoir si on lance une attaque ou non

        if (!aerialAttack)
        {
            if (character.inputPlayer.GetButtonDown("WeakAttack"))
                return ActionAttack(character, aerialWeakAttackFirstHit);

            if (character.inputPlayer.GetButtonDown("StrongAttack"))
                return ActionAttack(character, aerialHeavyAttack);
        }
        return false;
    }

    public bool ActionAttack(CharacterBase character, AttackManager attack)
    {
        if (attack == null)
            return false;
        if (character.Action.Action(attack) == true)
        {
            character.SetState(stateAction);
            //if (character.Input.inputActions.Count != 0)
            //	character.Input.inputActions[0].timeValue = 0;
            return true;
        }
        return false;
    }
}
