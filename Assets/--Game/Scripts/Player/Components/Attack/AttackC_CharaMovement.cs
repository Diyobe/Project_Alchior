using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


// Ptet a bouger pour les states machines pour chaque action ?
public class AttackC_CharaMovement : AttackComponent
{
    [Title("Movement X")]
    [SerializeField]
    bool keepMomentumX = false;


    [HideIf("keepMomentumX")]
    [HorizontalGroup("MomentumX")]
    [SerializeField]
    bool setMomentumX = false;

    [HorizontalGroup("MomentumX")]
    [HideIf("keepMomentumX")]
    [SerializeField]
    float momentumX = 0f;




    //[ShowIf("keepMomentumX")]
    [SerializeField]
    bool deccelerateX = false;

    [HorizontalGroup("DeccelerationX")]
    [ShowIf("deccelerateX")]
    [SerializeField]
    [HideLabel]
    AnimationCurve deccelerationCurveX;

    [HorizontalGroup("DeccelerationX", LabelWidth = 120)]
    [ShowIf("deccelerateX")]
    [SuffixLabel("en frames")]
    [SerializeField]
    float timeDeccelerationX = 10f;

    //---------------------------------------------------------
    [Title("Movement Z")]
    [SerializeField]
    bool keepMomentumZ = false;


    [HideIf("keepMomentumZ")]
    [HorizontalGroup("MomentumZ")]
    [SerializeField]
    bool setMomentumZ = false;

    [HorizontalGroup("MomentumZ")]
    [HideIf("keepMomentumZ")]
    [SerializeField]
    float momentumZ = 0f;


    [SerializeField]
    bool deccelerateZ = false;

    [HorizontalGroup("DeccelerationZ")]
    [ShowIf("deccelerateZ")]
    [SerializeField]
    [HideLabel]
    AnimationCurve deccelerationCurveZ;

    [HorizontalGroup("DeccelerationZ", LabelWidth = 120)]
    [ShowIf("deccelerateZ")]
    [SuffixLabel("en frames")]
    [SerializeField]
    float timeDeccelerationZ = 10f;
    //--------------------------------------------------------

    [Title("Movement Y")]
    [SerializeField]
    bool keepMomentumY = false;

    [HorizontalGroup("Gravity")]
    [SerializeField]
    bool applyGravity = false;

    [HorizontalGroup("Gravity")]
    [ShowIf("applyGravity")]
    [SerializeField]
    float gravityMultiplier = 1f;

    [HorizontalGroup("GravityDescend")]
    [SerializeField]
    bool applyGravityDescend = false;

    [HorizontalGroup("GravityDescend")]
    [ShowIf("applyGravityDescend")]
    [SerializeField]
    float gravityMultiplierDescend = 1f;

    [SerializeField]
    bool canAirControl = false;



    [Title("Ground Cancel")]
    [SerializeField]
    bool groundCancel = false;
    [SerializeField]
    [ShowIf("groundCancel")]
    [Tooltip("If not specified, return to idle")]
    private AttackManager groundEndLag;


    float timer = 0;
    float initialSpeedX = 0;
    float initialSpeedZ = 0;

    bool groundCancelNextFrame = false;


    public override void StartComponent(CharacterBase user)
    {
        if (keepMomentumX == false)
        {
            user.Movement.SpeedX = 0;
            if(setMomentumX == true)
                user.Movement.SpeedX = momentumX;

            user.Movement.SpeedY = 0;
        }

        if (keepMomentumZ == false)
        {
            user.Movement.SpeedZ = 0;
            if (setMomentumZ == true)
                user.Movement.SpeedZ = momentumZ;

            user.Movement.SpeedY = 0;
        }

        if (keepMomentumY == false)
            user.Movement.SpeedY = 0;

        timer = 0f;
        initialSpeedX = user.Movement.SpeedX;
        initialSpeedZ = user.Movement.SpeedZ;
        timeDeccelerationX /= 60f;
        timeDeccelerationZ /= 60f;

    }

    // jsp
    public override void UpdateComponent(CharacterBase user)
    {

        if (applyGravity == true)
        {
            if(applyGravityDescend && user.Movement.SpeedY < 0)
                user.Movement.ApplyGravityWithMultiplier(gravityMultiplierDescend);
            else
                user.Movement.ApplyGravityWithMultiplier(gravityMultiplier);
        }

        if (deccelerateX == true && timer < timeDeccelerationX)
        {
            timer += Time.deltaTime;
            user.Movement.SpeedX = initialSpeedX * deccelerationCurveX.Evaluate(timer / timeDeccelerationX);
            if (timer >= timeDeccelerationX)
                user.Movement.SpeedX = 0;
        }

        if (deccelerateZ == true && timer < timeDeccelerationZ)
        {
            timer += Time.deltaTime;
            user.Movement.SpeedZ = initialSpeedZ * deccelerationCurveZ.Evaluate(timer / timeDeccelerationZ);
            if (timer >= timeDeccelerationZ)
                user.Movement.SpeedZ = 0;
        }

      /*  if (accelerate == true && timer < timeDecceleration)
        {
            timer += Time.deltaTime * user.MotionSpeed;
            user.Movement.SpeedX = initialSpeedX * AccelerationCurve.Evaluate(timer / timeAcceleration);
            if (timer >= timeAcceleration)
                user.Movement.SpeedX = 0;
        }*/

        if (canAirControl == true)
            user.Movement.AirControl(user.inputPlayer.GetAxis("MoveX"), user.inputPlayer.GetAxis("MoveZ"));



        if(groundCancelNextFrame == true)
        {
            user.Action.CancelAction();
            user.ResetToLand();
        }

        if (groundCancel == true && user.Rigidbody.IsGrounded())
        {
            if (groundEndLag != null)
            {
                user.Action.CancelAction();
                user.Action.Action(groundEndLag);
            }
            else
            {
                groundCancelNextFrame = true;
            }
        }
        user.Movement.Move();
    }


    public override void OnHit(CharacterBase user, CharacterBase target)
    {

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
    public override void EndComponent(CharacterBase user)
    {

    }
}
