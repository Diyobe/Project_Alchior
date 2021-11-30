using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "Data/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public GameObject playerPrefab;
    public string characterName;

    public Sprite characterPortraitMenu;
    public GameObject characterMenuModel;

    [Header("Main Stats")]
    public int baseMaxHealthPoints = 500;
    [HideInInspector] public float currentHP = 10;
    public int baseMaxThermaPoints = 100;
    [HideInInspector] public float currentTH;
    [Space]
    public int baseAttack = 10;
    public int baseSpecialAttack = 10;
    public int baseDefense = 10;
    public int baseSpecialDefense = 10;

    [Header("Experience Stats")]
    [Range(1, 100)] public int level = 1;
    public float maxExperience = 3000;

    [Header("Hidden Stats")]
    public float baseStamina = 100;
    [Range(0, 100)] public float baseCriticalRate = 15;
    //à voir si on fait la speed

    [Header("Elemental Resistances")]
    [Range(0, 100)] public float basePyrosResistance = 0;
    [Range(0, 100)] public float baseNaturaResistance = 0;
    [Range(0, 100)] public float baseTerraResistance = 0;
    [Range(0, 100)] public float baseElectraResistance = 0;
    [Range(0, 100)] public float baseAerosResistance = 0;
    [Range(0, 100)] public float baseHydrosResistance = 0;

    [Space]
    [Header("Skill Pool")]
    public CharacterSkills skills;

    [Space]
    [Header("Equipment")]
    public WeaponType weaponType;
    public Weapon weapon;
    public ChestEquipment chestEquipment;
    public LegsEquipment legsEquipment;
    public AccessoryEquipment firstAccessory;
    public AccessoryEquipment secondAccessory;


    public delegate void OnStatChanged();
    public OnStatChanged onStatChangedCallback;


    public int GetMaxHP()
    {
        int maxHPAfterCalculation = baseMaxHealthPoints + (weapon ? weapon.maxHealthPointsModifier : 0)
                                                + (chestEquipment ? chestEquipment.maxHealthPointsModifier : 0) 
                                                + (legsEquipment ? legsEquipment.maxHealthPointsModifier : 0)
                                                + (firstAccessory ? firstAccessory.maxHealthPointsModifier : 0)
                                                + (secondAccessory ? secondAccessory.maxHealthPointsModifier : 0);
        return maxHPAfterCalculation;
    }

    public int GetMaxTH()
    {
        int maxTHAfterCalculation = baseMaxThermaPoints + (weapon ? weapon.maxThermaPointsModifier : 0)
                                                + (chestEquipment ? chestEquipment.maxThermaPointsModifier : 0)
                                                + (legsEquipment ? legsEquipment.maxThermaPointsModifier : 0)
                                                + (firstAccessory ? firstAccessory.maxThermaPointsModifier : 0)
                                                + (secondAccessory ? secondAccessory.maxThermaPointsModifier : 0);
        return maxTHAfterCalculation;
    }

    public int GetAttack()
    {
        int attackAfterCalculation = baseAttack + (weapon ? weapon.attackModifier : 0)
                                                + (chestEquipment ? chestEquipment.attackModifier : 0) 
                                                + (legsEquipment ? legsEquipment.attackModifier : 0)
                                                + (firstAccessory ? firstAccessory.attackModifier : 0)
                                                + (secondAccessory ? secondAccessory.attackModifier : 0);
        return attackAfterCalculation;
    }

    public int GetDefense()
    {
        int defenseAfterCalculation = baseDefense + (weapon ? weapon.defenseModifier : 0)
                                                + (chestEquipment ? chestEquipment.defenseModifier : 0)
                                                + (legsEquipment ? legsEquipment.defenseModifier : 0)
                                                + (firstAccessory ? firstAccessory.defenseModifier : 0)
                                                + (secondAccessory ? secondAccessory.defenseModifier : 0);
        return defenseAfterCalculation;
    }

    public int GetSpecialAttack()
    {
        int specialAttackAfterCalculation = baseSpecialAttack + (weapon ? weapon.specialAttackModifier : 0)
                                                + (chestEquipment ? chestEquipment.specialAttackModifier : 0) 
                                                + (legsEquipment ? legsEquipment.specialAttackModifier : 0)
                                                + (firstAccessory ? firstAccessory.specialAttackModifier : 0)
                                                + (secondAccessory ? secondAccessory.specialAttackModifier : 0);
        return specialAttackAfterCalculation;
    }

    public int GetSpecialDefense()
    {
        int specialDefenseAfterCalculation = baseSpecialDefense + (weapon ? weapon.specialDefenseModifier : 0)
                                                + (chestEquipment ? chestEquipment.specialDefenseModifier : 0)
                                                + (legsEquipment ? legsEquipment.specialDefenseModifier : 0)
                                                + (firstAccessory ? firstAccessory.specialDefenseModifier : 0)
                                                + (secondAccessory ? secondAccessory.specialDefenseModifier : 0);
        return specialDefenseAfterCalculation;
    }

    public void RegainHealthPoints(int amount)
    {
        if (currentHP + amount < baseMaxHealthPoints)
            currentHP += amount;
        else
            currentHP = baseMaxHealthPoints;

        onStatChangedCallback?.Invoke();
    }

    public void RegainThermaPoints(int amount)
    {
        if (currentTH + amount < baseMaxThermaPoints)
            currentTH += amount;
        else
            currentTH = baseMaxThermaPoints;

        onStatChangedCallback?.Invoke();
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP < 0)
            currentHP = 0;
    }
}
