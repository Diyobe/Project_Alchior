using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Legs Equipment", menuName = "Inventory/Legs Equipment")]
public class LegsEquipment : Equipment
{
    public LegsEquipment()
    {
        equipmentType = EquipmentType.LEGS;
    }
}
