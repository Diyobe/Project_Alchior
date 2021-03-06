using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] CharacterRigibody characterRigibody;

    public Animator animator;

    [Header("MovementSpeed")]
    public float walkingSpeed = 1.5f;
    public float movementSpeed = 5;
    public float runningSpeed = 7;

    [Title("Air Control")]
    [SerializeField]
    float airControl = 20f;
    [SerializeField]
    float airFriction = 0.9f;
    [SerializeField]
    private float maxAerialSpeed = 10f;
    public float MaxAerialSpeed
    {
        get { return maxAerialSpeed; }
        //set { maxAerialSpeed = value; }
    }

    [Title("Aerial")]
    [SerializeField]
    private float jumpForce;
    public float JumpForce
    {
        get { return jumpForce; }
    }

    [SerializeField]
    [ReadOnly]
    protected float speedX = 0;
    public float SpeedX
    {
        get { return speedX; }
        set { speedX = value; }
    }

    [SerializeField]
    [ReadOnly]
    protected float speedY = 0;
    public float SpeedY
    {
        get { return speedY; }
        set { speedY = value; }
    }

    [SerializeField]
    [ReadOnly]
    protected float speedZ = 0;
    public float SpeedZ
    {
        get { return speedZ; }
        set { speedZ = value; }
    }

    protected float motionSpeed = 1;
    public float MotionSpeed
    {
        get { return motionSpeed; }
        set { motionSpeed = value; }
    }

    float groundedGravity = 0.5f;
    public float gravity = 30f;

    [SerializeField] float fallSpeed = 6f;

    [SerializeField] float slopeForce;
    [SerializeField] float slopeForceRayLength;

    public bool isJumping;

    public void HandleMovement(float _dirX, float _dirZ)
    {
        Vector3 forward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
        Vector3 right = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);

        //projection de vecteur sur un autre plan
        Vector3 dir = forward * _dirZ + right * _dirX;
        dir.Normalize();

        //Vector3 movement;
        if (Mathf.Clamp01(Mathf.Abs(_dirX) + Mathf.Abs(_dirZ)) >= 0.5f)
        {
            speedX = dir.x * movementSpeed;
            speedZ = dir.z * movementSpeed;
        }
        else
        {
            speedX = dir.x * walkingSpeed;
            speedZ = dir.z * walkingSpeed;

        }
        characterRigibody.HandleMovement(speedX * motionSpeed, speedY * motionSpeed, speedZ * motionSpeed);

        if (_dirX != 0 || _dirZ != 0)
        {
            characterRigibody.HandleRotation(dir.x, dir.z);
            if (OnSlope())
                characterRigibody.characterController.Move(Vector3.down * characterRigibody.characterController.height / 2 * slopeForce * Time.deltaTime);
        }
    }

    public void Move()
    {
        characterRigibody.HandleMovement(speedX * motionSpeed, speedY * motionSpeed, speedZ * motionSpeed);
    }

    public void AirControl(float _dirX, float _dirZ)
    {
        Vector3 forward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
        Vector3 right = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);

        //projection de vecteur sur un autre plan
        Vector3 dir = forward * _dirZ + right * _dirX;

        speedX += (airControl * dir.x * airFriction) * Time.deltaTime;
        speedX = Mathf.Clamp(speedX, -maxAerialSpeed, maxAerialSpeed);

        speedZ += (airControl * dir.z * airFriction) * Time.deltaTime;
        speedZ = Mathf.Clamp(speedZ, -maxAerialSpeed, maxAerialSpeed);
        characterRigibody.HandleMovement(speedX * motionSpeed, speedY * motionSpeed, speedZ * motionSpeed);

    }
    public void MoveForward(float multiplier)
    {
        characterRigibody.HandleMovement(walkingSpeed * characterRigibody.transform.forward.x * multiplier * motionSpeed,
                                                        speedY,
                                                        walkingSpeed * characterRigibody.transform.forward.z * multiplier * motionSpeed);
    }

    public void MoveBackward(float multiplier)
    {
        characterRigibody.HandleMovement(walkingSpeed * -characterRigibody.transform.forward.x * multiplier * motionSpeed,
                                                        speedY,
                                                        -walkingSpeed * characterRigibody.transform.forward.z * multiplier * motionSpeed);
    }

    public void Launched(float multiplier)
    {
        speedY = jumpForce * multiplier;
        speedX = -characterRigibody.transform.forward.x * multiplier * motionSpeed;
        speedZ = -characterRigibody.transform.forward.z * multiplier * motionSpeed;
        //characterRigibody.HandleMovement(-characterRigibody.transform.forward.x * multiplier * motionSpeed,
        //                                                0,
        //                                                -characterRigibody.transform.forward.z * multiplier * motionSpeed);
    }

    public void Spiked(float multiplier)
    {
        speedY = -gravity * multiplier;
        //characterRigibody.HandleMovement(-characterRigibody.transform.forward.x * multiplier * motionSpeed,
        //                                                0,
        //                                                -characterRigibody.transform.forward.z * multiplier * motionSpeed);
    }

    public void LookAt(Vector3 vector)
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = vector.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = vector.z;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        characterRigibody.transform.rotation = targetRotation;
    }

    public void HandleGravity()
    {
        float gravityLimit;

        if (characterRigibody.IsGrounded())
        {
            gravityLimit = groundedGravity;
        }
        else
        {
            gravityLimit = gravity;
        }

        if (speedY > -gravityLimit)
            speedY -= Time.deltaTime * fallSpeed;
        else
            speedY = -gravityLimit;
    }

    public void ApplyGravityWithMultiplier(float multiplier)
    {
        float gravityLimit;

        if (characterRigibody.IsGrounded())
        {
            gravityLimit = groundedGravity;
        }
        else
        {
            gravityLimit = gravity;
        }

        if (speedY > -gravityLimit)
            speedY -= Time.deltaTime * (fallSpeed * multiplier);
        else
            speedY = -gravityLimit;
    }

    public void Jump()
    {
        Jump(jumpForce);
    }

    public void Jump(float jumpForce)
    {
        isJumping = true;
        speedY = jumpForce;
        //characterRigibody.HandleMovement(speedX, speedY, speedZ);
        //characterParticle.UseParticle("jump");
    }

    bool OnSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, characterRigibody.characterController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }

    public void ResetAnimator()
    {
        animator.SetBool("Run", false);
        animator.ResetTrigger("DoubleJump");
        animator.ResetTrigger("Crouch");
        animator.ResetTrigger("Landing");
        animator.ResetTrigger("Jumping");
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Knockback");
        animator.ResetTrigger("Fall");
    }
}
