using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum KnockbackType
{
    Knockback,
    Knockdown,
    Launcher,
    Spike
}

public class AttackC_KnockbackSpeed : AttackComponent
{
    [Title("HitStop")]
    [SerializeField]
    float hitStopDuration = 0.1f;

    [Title("Launch Distance")]
    [SerializeField]
    float launchDistance = 1f;

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
        target.Movement.LookAt(user.transform.position);
        switch (knockbackType)
        {
            case KnockbackType.Knockback:
                target.Knockback.KnockbackDuration = .3f;
                target.Movement.MoveBackward(launchDistance);
                break;
            case KnockbackType.Launcher:
                target.Knockback.KnockbackDuration = 1f;
                target.Movement.Launched(launchDistance);
                break;
            case KnockbackType.Spike:
                target.Knockback.KnockbackDuration = 1f;
                target.Movement.Spiked(launchDistance);
                break;
        }

        if (hitStopUser)
            user.SetMotionSpeed(0f, hitStopDuration);
        if (hitStopTarget)
            target.SetMotionSpeed(0f, hitStopDuration);
    }
}
