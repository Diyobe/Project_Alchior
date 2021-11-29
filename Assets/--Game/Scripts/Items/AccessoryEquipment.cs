using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Accessory Equipment", menuName = "Inventory/Accessory Equipment")]
public class AccessoryEquipment : Equipment
{
    public AccessoryEquipment()
    {
        equipmentType = EquipmentType.ACCESSORY;
    }
}
