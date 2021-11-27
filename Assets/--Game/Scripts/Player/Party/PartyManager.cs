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

    public void EquipItem(CharacterBase character, Equipment equipment, bool isFirstAccessory)
    {
        if (party.Count <= 0) return;
        if (equipment is Weapon weapon)
        {
            if (weapon.weaponType == character.Stats.weaponType)
            {
                if (character.Stats.weapon != null)
                    Inventory.Instance.Add(character.Stats.weapon);

                character.Stats.weapon = weapon;
                Inventory.Instance.Remove(weapon);
            }
            else
                Debug.LogError("Can't equip this type of weapon to this character");
        }
        else
        {
            if (equipment.equipmentType == EquipmentType.CHEST)
            {
                if (character.Stats.chestEquipment != null)
                    Inventory.Instance.Add(character.Stats.chestEquipment);

                character.Stats.chestEquipment = equipment;
                Inventory.Instance.Remove(equipment);
            }
            else if (equipment.equipmentType == EquipmentType.LEGS)
            {
                if (character.Stats.legsEquipment != null)
                    Inventory.Instance.Add(character.Stats.legsEquipment);

                character.Stats.legsEquipment = equipment;
                Inventory.Instance.Remove(equipment);
            }
            else
            {
                if (isFirstAccessory)
                {
                    if (character.Stats.firstAccessory != null)
                        Inventory.Instance.Add(character.Stats.firstAccessory);

                    character.Stats.firstAccessory = equipment;
                    Inventory.Instance.Remove(equipment);
                }
                else
                {
                    if (character.Stats.secondAccessory != null)
                        Inventory.Instance.Add(character.Stats.secondAccessory);

                    character.Stats.secondAccessory = equipment;
                    Inventory.Instance.Remove(equipment);
                }
            }
        }
    }
}
