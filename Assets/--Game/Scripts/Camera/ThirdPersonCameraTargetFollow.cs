using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraTargetFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothSpeed = 0.125f;
    Vector3 velocity = Vector3.one;

    private void LateUpdate()
    {
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, target.position,ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
