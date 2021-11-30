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
	AttackManager groundedWeakAttackSecondHit = null;
	[SerializeField]
	AttackManager groundedWeakAttackLastHit = null;
	[SerializeField]
	AttackManager launcher = null;

	[Title("Parameter - Ground Heavy Attacks")]
	[SerializeField]
	AttackManager groundedHeavyAttack = null;

	[Title("Parameter - Aerial Weak Attacks")]
	[SerializeField]
	AttackManager aerialWeakAttackFirstHit = null;
	[SerializeField]
	AttackManager aerialWeakAttackSecondHit = null;
	[SerializeField]
	AttackManager aerialWeakAttackLastHit = null;

	[Title("Parameter - Aerial Heavy Attacks")]
	[SerializeField]
	AttackManager aerialHeavyAttack = null;


	[Title("States")]
	[SerializeField]
	CharacterState stateAction;

	public bool ActionAttackGrounded(CharacterBase character, bool canSpecial = true)
    {
		//Check des inputs pour savoir si on lance une attaque ou non
		if (character.inputPlayer.GetButtonDown("WeakAttack"))
			return ActionAttack(character, groundedWeakAttackFirstHit);
		return false;
	}

	public bool ActionAttackAerial(CharacterBase character, bool canSpecial = true)
	{
		//Check des inputs pour savoir si on lance une attaque ou non
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
