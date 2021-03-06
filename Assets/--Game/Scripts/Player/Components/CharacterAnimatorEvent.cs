using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sert de proxy pour les events d'animations
public class CharacterAnimatorEvent : MonoBehaviour
{
	[SerializeField]
	CharacterMovement characterMovement;

	[SerializeField]
	CharacterMoveset characterMoveset;

	[SerializeField]
	CharacterAction characterAction;

	//[SerializeField]
	//CharacterParry characterParry;

	public void MoveForward(float multiplier)
	{
		// Pour le forward tilt de crow pour empêcher de reset la speed a zero dans un jump cancel
		if (characterAction.CurrentAttackManager == null)
			return;
		characterMovement.MoveForward(multiplier);
	}
	public void Jump(float multiplier)
	{
		characterMovement.Jump(multiplier);
	}

	// Character Action
	public void ActionActive(int subAction = 0)
	{
		characterAction.ActionActive(subAction);
	}

	public void ActionUnactive(int subAction = 0)
	{
		characterAction.ActionUnactive(subAction);
	}
	public void ActionAllActive()
	{
		characterAction.ActionAllActive();
	}

	public void ActionAllUnactive()
	{
		characterAction.ActionAllUnactive();
	}

	public void MoveCancelable()
	{
		characterAction.MoveCancelable();
	}

	public void EndAction()
	{
		characterAction.EndAction();
	}

	public void EndActionAerial()
    {
		characterMoveset.aerialAttack = true;
		characterAction.EndAction();
    }

	public void MovementCancelable()
    {
		characterAction.movementCancelable = true;
    }

	public void ParryOn()
	{
		Debug.Log("Allo?");
		//characterParry.IsParry = true;
	}

	public void ParryOff()
	{
		//characterParry.IsParry = false;
	}

	public void PlaySound(string soundName)
	{
		// récupérer l'audio manager et jouer un son depuis le projet avec une string
	}



}
