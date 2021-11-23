using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRigidBodyEntity : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField]
    GameObject model3D;

    [SerializeField]
    LayerMask groundLayerMask;

    [SerializeField]
    LayerMask wallLayerMask;

    [SerializeField]
    BoxCollider boxCollider;

    [SerializeField]
    Transform characterAttackOrigin;

    [Header("Ground Movement")]
    public float horizontalSpeed = 0f;

    public float walkAcceleration = 1f;
    public float runAcceleration = 2f;
    public float aerialAcceleration = 0.5f;


    public float decceleration = 50f;

    public float walkSpeed = 10f;

    public float runSpeed = 15f;

    public bool isRunningInputPressed = false;

    float _dirX;
    float _dirZ;
    float movementX = 0;
    float movementZ = 0;

    Vector3 newVelocity;
    [Header("Collisions")]
    public bool activateCollisions = true;

    public float groundFriction = 10f;

    [Header("Gravity")]
    float gravity = 9.8f;

    float fallSpeedMultiplier = 2.7f;
    public float verticalSpeed;

    public float fallSpeedMax = 20;

    [Header("Jump")]
    [SerializeField] float jumpForce = 10f;
    bool isJumping = false;
    float jumpTiming = .1f;
    bool doubleJump = true;
    public float airFriction = 20f;

    [Header("Animations")]
    [SerializeField] Animator characterAnimator;

    public bool IsRunning(bool inputPressed, bool hasEnoughStamina = true)
    {
        return inputPressed && hasEnoughStamina;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckCeiling();
        if (!isJumping)
            UpdateGravity(gravity);
        else
            UpdateJump();


        UpdateMovement();
    }
    private void Update()
    {
        UpdateAnimation();
    }

    void UpdateGravity(float gravity)
    {
        if (!IsGrounded())
        {
            verticalSpeed -= gravity * (Time.fixedDeltaTime * fallSpeedMultiplier);
            if (verticalSpeed < -fallSpeedMax)
            {
                verticalSpeed = -fallSpeedMax;
            }
        }
        else
        {
            isJumping = false;
        }

    }

    private bool IsGrounded()
    {
        //on caste une box sur toute une distance (ici 0.5f) et quand le cast collide sur un objet du layermask on le recalle au point d'impact en ajoutant un léger offset sur le y pour éviter de passer
        //à travers le sol. (voir le box gizmo en dessous des pieds de ninjamurai)
        if (!activateCollisions) return false;

        float extraHeight = 0.01f;
        RaycastHit raycastHit;
        Color rayColor;
        if (Physics.BoxCast(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y, boxCollider.bounds.center.z), new Vector3(boxCollider.bounds.size.x, extraHeight, boxCollider.bounds.size.z), Vector3.down, out raycastHit, Quaternion.identity, 0.5f/* + Mathf.Abs(verticalSpeed)*/, groundLayerMask))
        {
            //raycastHit.
            rayColor = Color.green;
            verticalSpeed = 0;
            transform.position = new Vector3(transform.position.x, transform.position.y - (raycastHit.distance - extraHeight), transform.position.z);
            //Vector3 newPos = transform.position;
            //transform.position = newPos;
            doubleJump = false;
            return true;
        }
        else
        {
            rayColor = Color.red;
            return false;
        }
    }

    private void CheckCeiling()
    {
        if (!activateCollisions) return;

        float extraHeight = 0.01f;
        RaycastHit raycastHit;

        if (Physics.BoxCast(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.max.y, boxCollider.bounds.center.z), new Vector3(boxCollider.bounds.size.x, extraHeight, boxCollider.bounds.size.z), Vector3.up, out raycastHit, Quaternion.identity, 0.5f/* + Mathf.Abs(verticalSpeed)*/, wallLayerMask))
        {
            if (verticalSpeed > 0)
                verticalSpeed = 0;
            transform.position = new Vector3(transform.position.x, transform.position.y - (raycastHit.distance - extraHeight), transform.position.z);
        }
    }

    private void CheckCollisionsX()
    {
        if (!activateCollisions) return;

        float extraHeight = 0.01f;
        float offsetY = 0.03f;
        RaycastHit raycastHit;

        if (Physics.BoxCast(new Vector3(boxCollider.bounds.max.x, boxCollider.bounds.center.y, boxCollider.bounds.center.z),
            new Vector3(extraHeight, boxCollider.bounds.size.y - offsetY, boxCollider.bounds.size.z),
            Vector3.right, out raycastHit, Quaternion.identity, 0.4f, wallLayerMask))
        {
            if (movementX > 0)
                movementX = 0;

            if (boxCollider.bounds.size.y - raycastHit.point.y <= offsetY)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
            }
            //transform.position = new Vector3(transform.position.x - (raycastHit.distance - extraHeight), transform.position.y, transform.position.z);
        }
        else if (Physics.BoxCast(new Vector3(boxCollider.bounds.min.x, boxCollider.bounds.center.y, boxCollider.bounds.center.z),
            new Vector3(extraHeight, boxCollider.bounds.size.y - offsetY, boxCollider.bounds.size.z),
            Vector3.left, out raycastHit, Quaternion.identity, 0.4f, wallLayerMask))
        {
            if (movementX < 0)
                movementX = 0;

            if (boxCollider.bounds.size.y - raycastHit.point.y <= offsetY)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
            }
            //transform.position = new Vector3(transform.position.x - (raycastHit.distance - extraHeight), transform.position.y, transform.position.z);
        }
    }

    private void CheckCollisionsZ()
    {
        if (!activateCollisions) return;

        float extraHeight = 0.01f;
        float offsetY = 0.03f;
        RaycastHit raycastHit;

        if (Physics.BoxCast(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.center.y, boxCollider.bounds.max.z),
            new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y - offsetY, extraHeight),
            Vector3.forward, out raycastHit, Quaternion.identity, 0.4f, wallLayerMask))
        {
            if (movementZ > 0)
                movementZ = 0;

            if (boxCollider.bounds.size.y - raycastHit.point.y <= offsetY)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
            }
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (raycastHit.distance - extraHeight));
        }
        else if (Physics.BoxCast(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.center.y, boxCollider.bounds.min.z),
            new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y - offsetY, extraHeight),
            Vector3.back, out raycastHit, Quaternion.identity, 0.4f, wallLayerMask))
        {
            if (movementZ < 0)
                movementZ = 0;

            if (boxCollider.bounds.size.y - raycastHit.point.y <= offsetY)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
            }
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (raycastHit.distance - extraHeight));
        }
    }
    //private void OnDrawGizmos()
    //{
    //    //------------------------------------------------------------
    //    // PERMET DE VOIR LA BOX DE DETECTION DES COLLISIONS Z

    //    //Gizmos.DrawCube(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.center.y, boxCollider.bounds.max.z),
    //    //    new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y, 0.01f));

    //    //Gizmos.DrawCube(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.center.y, boxCollider.bounds.min.z),
    //    //    new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y, 0.01f));

    //    //------------------------------------------------------------
    //    // PERMET DE VOIR LA BOX DE DETECTION DES COLLISIONS Z

    //    //Gizmos.DrawCube(new Vector3(boxCollider.bounds.min.x, boxCollider.bounds.center.y, boxCollider.bounds.center.z),
    //    //    new Vector3(0.01f, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

    //    //Gizmos.DrawCube(new Vector3(boxCollider.bounds.max.x, boxCollider.bounds.center.y, boxCollider.bounds.center.z),
    //    //    new Vector3(0.01f, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    //}

    void UpdatePosition()
    {
        Vector3 newPos = transform.position;
        newPos.x = movementX;
        newPos.y = verticalSpeed;
        newPos.z = movementZ;
        rb.velocity = newPos;
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            Debug.Log("Jump");
            isJumping = true;
            jumpTiming = .1f;
            verticalSpeed = 10;
        }
        else
        {
            if (!doubleJump)
            {
                Debug.Log("Double Jump");
                characterAnimator.SetTrigger("DoubleJump");
                isJumping = true;
                jumpTiming = .1f;
                verticalSpeed = 10;
                doubleJump = true;
                newVelocity = new Vector3(_dirX * walkSpeed, verticalSpeed, _dirZ * walkSpeed);
                rb.velocity = newVelocity;
            }
        }

    }

    void UpdateJump()
    {
        jumpTiming -= Time.deltaTime;
        if (jumpTiming <= 0)
            isJumping = false;
    }

    public void Move(float dirX, float dirZ)
    {
        //_dirX = dirX;
        //_dirZ = dirZ;

        //on met le vecteur Y à 0 pour pas foncer vers le sol ou dans les airs quand on bouge la camera en y
        Vector3 forward = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
        Vector3 right = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);

        //projection de vecteur sur un autre plan
        Vector3 dir = forward * dirZ + right * dirX;

        _dirX = dir.x;
        _dirZ = dir.z;

        if (dirX != 0 || dirZ != 0)
        {
            //enlever les tweens, ça fait de la merde
            //model3D.transform.DORotateQuaternion(Quaternion.LookRotation(dir, Vector3.up), 0.4f);
            //boxCollider.transform.DORotateQuaternion(Quaternion.LookRotation(dir, Vector3.up), 0.4f);
            Vector3 newDirection = Vector3.RotateTowards(model3D.transform.forward, dir, Time.deltaTime * 15f, 0.0f);

            //transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up); 
            model3D.transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up);
            boxCollider.transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up);
            characterAttackOrigin.rotation = Quaternion.LookRotation(newDirection, Vector3.up);
            //if (rb.velocity.y == 0)
            //{
            //    
            //}

        }

        //Quaternion.LookRotation(dir, Vector3.up);
    }

    void UpdateAnimation()
    {
        //characterAnimator.SetBool("Move", _dirX != 0 || _dirZ != 0);
        characterAnimator.SetFloat("Blend", new Vector2(rb.velocity.x, rb.velocity.z).magnitude / walkSpeed);
        characterAnimator.SetBool("Run", IsRunning(isRunningInputPressed));
        characterAnimator.SetBool("IsGrounded", verticalSpeed == 0);
    }

    void UpdateMovement()
    {
        // à Changer de toute urgence, regarder les liens que Nam à envoyé
        if (!IsRunning(isRunningInputPressed))
        {
            if (Mathf.Abs(_dirX) > 0.1f || Mathf.Abs(_dirZ) > 0.1f)
            {
                if (horizontalSpeed < walkSpeed)
                    horizontalSpeed += ((rb.velocity.y == 0) ? walkAcceleration : aerialAcceleration) * Time.deltaTime;
                else
                {
                    //if (horizontalSpeed > walkSpeed)
                    //{
                    //    horizontalSpeed -= ((rb.velocity.y == 0) ? decceleration : decceleration) * Time.deltaTime;
                    //}else
                    horizontalSpeed = walkSpeed;

                }
            }
            else
            {
                if (horizontalSpeed > 0)
                    horizontalSpeed -= ((rb.velocity.y == 0) ? decceleration : decceleration) * Time.deltaTime;
                else
                    horizontalSpeed = 0;
            }

            //----------------------------------------------------

            if (rb.velocity.y == 0)
            {
                movementX = _dirX * horizontalSpeed;
                movementZ = _dirZ * horizontalSpeed;
            }

        }
        else
        {
            if (horizontalSpeed < runSpeed)
                horizontalSpeed += runAcceleration * Time.deltaTime;

            if (rb.velocity.y == 0)
            {
                movementX = boxCollider.transform.forward.x * horizontalSpeed;
                movementZ = boxCollider.transform.forward.z * horizontalSpeed;
            }

            //if (Mathf.Abs(_dirX) > 0.1f || Mathf.Abs(_dirZ) > 0.1f)
            //{
            //    if (horizontalSpeed < runSpeed)
            //        horizontalSpeed += ((rb.velocity.y == 0) ? decceleration : decceleration) * Time.deltaTime;
            //}
            //else
            //{
            //    if (horizontalSpeed > 0)
            //        horizontalSpeed -= ((rb.velocity.y == 0) ? decceleration : decceleration) * Time.deltaTime;
            //    else
            //        horizontalSpeed = 0;
            //}
        }

        if (rb.velocity.y != 0)
        {
            float aerialSpeedLimit = walkSpeed;
            if (isRunningInputPressed)
                aerialSpeedLimit = runSpeed;
            else
                aerialSpeedLimit = walkSpeed;

            if (Mathf.Abs(movementX) + Mathf.Abs(movementZ) < aerialSpeedLimit)
            {
                movementX += _dirX * aerialAcceleration * Time.deltaTime;
                movementZ += _dirZ * aerialAcceleration * Time.deltaTime;
            }
            else
            {
                if (Mathf.Sign(movementX) != Mathf.Sign(_dirX))
                    movementX += _dirX * aerialAcceleration * Time.deltaTime;

                if (Mathf.Sign(movementZ) != Mathf.Sign(_dirZ))
                    movementZ += _dirZ * aerialAcceleration * Time.deltaTime;
            }
        }

        CheckCollisionsX();
        CheckCollisionsZ();
        newVelocity = new Vector3(movementX, verticalSpeed, movementZ);
        rb.velocity = newVelocity;
    }

    private float _GetFriction()
    {
        return IsGrounded() ? groundFriction : airFriction;
    }
}
