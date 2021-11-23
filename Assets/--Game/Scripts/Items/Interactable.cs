using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    bool hasInteracted = false; 

    public virtual void Interact()
    {
        Debug.Log("Interacting with " + name);
        hasInteracted = true;
    }
}
