using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    [Header("Categories")]
    public EquipmentType equipmentType;
    public User user;

    [Header("Stats Modifiers")]
    public int maxHealthPointsModifier = 0;
    public int maxThermaPointsModifier = 0;
    public int maxStaminaModifier = 0;
    public int attackModifier = 0;
    public int specialAttackModifier = 0;
    public int defenseModifier = 0;
    public int specialDefenseModifier = 0;
    public int speedModifier = 0;

    [Header("Elemental Resistances Modifiers")]
    public float pyrosResistanceModifier = 0;
    public float NaturaResistanceModifier = 0;
    public float TerraResistanceModifier = 0;
    public float ElectraResistanceModifier = 0;
    public float AerosResistanceModifier = 0;
    public float HydrosResistanceModifier = 0;

    [Header("Other Modifiers")]
    public float criticalRateModifier = 0;
    [Range(1, 5)]public float experienceGainMultiplier = 1;

    public override void Use()
    {
        base.Use();
        //Check which type of equipment it is
        //Equip the item at the given slot
        //If there was an equipment before, add the previous equipment to inventory
        //Remove it from the inventory
    }

    public virtual void Equip(CharacterData character, bool isFirstAccessory)
    {
        Debug.Log(character.characterName + " is now equiped with " + name);
        PartyManager.Instance.EquipItem(character, this, isFirstAccessory);
    }
}

public enum EquipmentType
{
    WEAPON,
    CHEST,
    LEGS,
    ACCESSORY,
}

public enum User
{
    BOTH,
    MALE,
    FEMALE,
}
