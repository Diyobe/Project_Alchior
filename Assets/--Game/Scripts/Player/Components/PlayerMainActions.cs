using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainActions : MonoBehaviour
{
    [SerializeField] InteractableDetector interactableDetector;

    public void Interact()
    {
        if (GameManager.Instance.gamePaused) return;

        if(interactableDetector.targetInteractable != null)
        {
            interactableDetector.targetInteractable.Interact();
        }
    }
}
