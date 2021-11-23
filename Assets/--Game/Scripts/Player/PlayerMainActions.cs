using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainActions : MonoBehaviour
{
    [SerializeField] InteractableDetector interactableDetector;

    public bool isInMenu = false;

    public void Interact()
    {
        if (isInMenu) return;

        if(interactableDetector.targetInteractable != null)
        {
            interactableDetector.targetInteractable.Interact();
        }
    }
}
