using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable
{
    [SerializeField] Item item;
    [SerializeField] uint quantity = 1;

    public override void Interact()
    {
        base.Interact();
        Pickup();
    }

    void Pickup()
    {
        Debug.Log("Picking up " + item.name);
        if (Inventory.Instance.Add(item, quantity))
            Destroy(gameObject);
    }
}
