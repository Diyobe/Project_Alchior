using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    #region Singleton Pattern
    // Static singleton instance
    private static PartyManager instance;

    // Static singleton property
    public static PartyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PartyManager>();
                if (instance == null)
                {
                    instance = new GameObject("PartyManager").AddComponent<PartyManager>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null) Destroy(this);

        DontDestroyOnLoad(gameObject);//le GameObject qui porte ce script ne sera pas détruit
    }

    #endregion Singleton Pattern

    public List<CharacterData> characterDatas = new List<CharacterData>();
    public List<CharacterBase> party = new List<CharacterBase>();
    public GameObject currentCharacterGameObject;

    public delegate void OnEquipmentChanged(Equipment newEquipment, Equipment oldEquipment);
    public OnEquipmentChanged onEquipmentChanged;

    private void Start()
    {
        //party.Clear();
        //if (currentCharacterGameObject != null) Destroy(currentCharacterGameObject.gameObject);

        //foreach(CharacterData data in characterDatas)
        //{
        //    party.Add(data.character);
        //}
        //currentCharacterGameObject = Instantiate(characterDatas[0].playerPrefab);
    }

    public void Unequip(CharacterData character, Equipment equipment, bool isFirstAccessory)
    {
        if (equipment == null) return;
        if (equipment.equipmentType == EquipmentType.WEAPON) return;

        switch (equipment.equipmentType)
        {
            case EquipmentType.CHEST:
                character.chestEquipment = null;
                break;
            case EquipmentType.LEGS:
                character.legsEquipment = null;
                break;
            case EquipmentType.ACCESSORY:
                if (isFirstAccessory)
                    character.firstAccessory = null;
                else
                    character.secondAccessory = null;
                break;
        }
        Equipment oldEquipment = equipment;
        Inventory.Instance.Add(equipment);
        onEquipmentChanged?.Invoke(null, oldEquipment);
    }

    public void EquipItem(CharacterData character, Equipment equipment, bool isFirstAccessory)
    {
        if (party.Count <= 0) return;
        Equipment oldEquipment = null;

        if (equipment is Weapon weapon)
        {
            if (weapon.weaponType == character.weaponType)
            {
                if (character.weapon != null)
                {
                    oldEquipment = character.weapon;
                    Inventory.Instance.Add(character.weapon);
                }

                character.weapon = weapon;
                Inventory.Instance.Remove(weapon);
            }
            else
                Debug.LogError("Can't equip this type of weapon to this character");
        }
        else
        {
            if (equipment.equipmentType == EquipmentType.CHEST)
            {
                if (character.chestEquipment != null)
                {
                    oldEquipment = character.chestEquipment;
                    Inventory.Instance.Add(character.chestEquipment);
                }

                character.chestEquipment = equipment;
                Inventory.Instance.Remove(equipment);
            }
            else if (equipment.equipmentType == EquipmentType.LEGS)
            {
                if (character.legsEquipment != null)
                {
                    oldEquipment = character.legsEquipment;
                    Inventory.Instance.Add(character.legsEquipment);
                }

                character.legsEquipment = equipment;
                Inventory.Instance.Remove(equipment);
            }
            else
            {
                if (isFirstAccessory)
                {
                    if (character.firstAccessory != null)
                    {
                        oldEquipment = character.firstAccessory;
                        Inventory.Instance.Add(character.firstAccessory);
                    }

                    character.firstAccessory = equipment;
                    Inventory.Instance.Remove(equipment);
                }
                else
                {
                    if (character.secondAccessory != null)
                    {
                        oldEquipment = character.secondAccessory;
                        Inventory.Instance.Add(character.secondAccessory);
                    }

                    character.secondAccessory = equipment;
                    Inventory.Instance.Remove(equipment);
                }
            }
        }

        onEquipmentChanged?.Invoke(equipment, oldEquipment);
    }
}
