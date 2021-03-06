using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AttackC_JumpCancel : AttackComponent
{
	[SerializeField]
	bool jumpCancelOnHit = false;

	bool autoCancel = false;

	// Appelé au moment où l'attaque est initialisé
	public override void StartComponent(CharacterBase user)
    {
	}


	// Appelé tant que l'attaque existe 
	//(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
	public override void UpdateComponent(CharacterBase user)
    {
		if (user.Action.CanAct() || autoCancel)
		{
			if (user.inputPlayer.GetButtonDown("Jump"))
			{
				user.Action.FinishAction();
				//user.Movement.Jump();
			}
		}
    }
	
	// Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
		if (jumpCancelOnHit)
			autoCancel = true;

	}

	public override void OnParry(CharacterBase user, CharacterBase target)
	{

	}
	public override void OnGuard(CharacterBase user, CharacterBase target, bool guardRepel)
	{

	}
	public override void OnClash(CharacterBase user, CharacterBase target)
	{

	}

	// Appelé au moment de la destruction de l'attaque
	public override void EndComponent(CharacterBase user)
    {
		
    }
}
