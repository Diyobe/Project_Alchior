using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest Equipment", menuName = "Inventory/Chest Equipment")]
public class ChestEquipment : Equipment
{
    public ChestEquipment()
    {
        equipmentType = EquipmentType.CHEST;
    }
}
