using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterRigibody : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float verticalSpeed = 10f;
    public CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void HandleRotation(float _dirX, float _dirZ)
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _dirX;
        positionToLookAt.y = 0f;
        positionToLookAt.z = _dirZ;

        Quaternion currentRotation = transform.rotation;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void HandleMovement(float movementX, float movementY, float movementZ)
    {
        Vector3 newMovement;
        newMovement.x = movementX;
        newMovement.y = movementY;
        newMovement.z = movementZ;
        characterController.Move(newMovement * Time.deltaTime);
    }

    [ShowInInspector]
    public bool IsGrounded()
    {
        return characterController.isGrounded;
    }
}
