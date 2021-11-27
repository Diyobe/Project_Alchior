using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterStats : MonoBehaviour
{
    [Title("Main Stats")]
    public uint maxHealthPoints = 500;
    [HideInInspector] public float currentHP;
    public uint maxThermaPoints = 100;
    [HideInInspector] public float currentTH;
    [Space]
    public uint attack = 10;
    public uint specialAttack = 10;
    public uint defense = 10;
    public uint specialDefense = 10;

    [Title("Experience Stats")]
    [Range(1, 100)] public int level = 1;
    public float maxExperience = 3000;

    [Title("Hidden Stats")]
    public float stamina = 100;
    [Range(0, 100)] public float criticalRate = 15;
    //à voir si on fait la speed

    [Title("Elemental Resistances")]
    [Range(0, 100)] public float pyrosResistance = 0;
    [Range(0, 100)] public float naturaResistance = 0;
    [Range(0, 100)] public float terraResistance = 0;
    [Range(0, 100)] public float electraResistance = 0;
    [Range(0, 100)] public float aerosResistance = 0;
    [Range(0, 100)] public float hydrosResistance = 0;

    [Space]
    [Title("Equipment")]
    public WeaponType weaponType;
    public Weapon weapon;
    public Equipment chestEquipment;
    public Equipment legsEquipment;
    public Equipment firstAccessory;
    public Equipment secondAccessory;
}
