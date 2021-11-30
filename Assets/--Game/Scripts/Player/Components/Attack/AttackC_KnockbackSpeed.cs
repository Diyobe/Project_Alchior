using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum KnockbackType
{
    Knockback,
    Knockdown,
    Launcher
}

public class AttackC_KnockbackSpeed : AttackComponent
{
    [Title("HitStop")]
    [SerializeField]
    float hitStopDuration = 0.1f;

    [SerializeField]
    KnockbackType knockbackType;

    [SerializeField] bool hitStopUser = true;
    [SerializeField] bool hitStopTarget = true;



    // Appelé au moment où l'attaque est initialisé
    public override void StartComponent(CharacterBase user)
    {

    }

    // Appelé tant que l'attaque existe 
    //(Peut-être remplacé par l'Update d'Unity de base si l'ordre d'éxécution n'est pas important)
    public override void UpdateComponent(CharacterBase user)
    {

    }

    // Appelé au moment où l'attaque touche une target
    public override void OnHit(CharacterBase user, CharacterBase target)
    {
        Debug.LogError(user.CharacterData.name + " has hit " + target.CharacterData.name);
        target.Knockback.KnockbackDuration = .3f;
        switch (knockbackType)
        {
            case KnockbackType.Knockback:
                target.Movement.MoveBackward(1);
                break;
        }

        if (hitStopUser)
            user.SetMotionSpeed(0f, hitStopDuration);
        if (hitStopTarget)
            target.SetMotionSpeed(0f, hitStopDuration);
    }
}
