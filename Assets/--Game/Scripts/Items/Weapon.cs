using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Equipment
{
    [Header("Categories")]
    public WeaponType weaponType;
    public override void Use()
    {
        base.Use();
        //Check if the weapon type corresponds to the character, if true equip at the given slot
    }
}

public enum WeaponType { KATANA, RIFLE, KNUCKLES, GREATSWORD, DAGGERS, POLEARM, NINJATO}
