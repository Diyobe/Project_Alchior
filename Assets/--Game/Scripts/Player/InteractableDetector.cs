using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    public Interactable targetInteractable = null;
    bool interacatbleDetected = false;


    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            targetInteractable = interactable;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null && targetInteractable != null && interactable != targetInteractable)
        {
            if (!interacatbleDetected)
                interacatbleDetected = true;

            if (Vector3.Distance(transform.position, targetInteractable.transform.position) > Vector3.Distance(transform.position, interactable.transform.position))
            {
                targetInteractable = interactable;
            }
        }
        if(targetInteractable == null && interactable != null)
        {
            targetInteractable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
            if(!interacatbleDetected)
            targetInteractable = null;

            if (interacatbleDetected)
                interacatbleDetected = false;

        }
    }
}
