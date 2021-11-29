using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public Item Item;
    public uint Amount;

    public InventoryItem(Item item, uint amount)
    {
        Item = item;
        Amount = amount;
    }
}

public class InventoryEquipment
{
    public Equipment Equipment;
    public uint Amount;

    public InventoryEquipment(Equipment equipment, uint amount)
    {
        Equipment = equipment;
        Amount = amount;
    }
}

public class Inventory : MonoBehaviour
{

    #region Singleton Pattern
    // Static singleton instance
    private static Inventory instance;

    // Static singleton property
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Inventory>();
                if (instance == null)
                {
                    instance = new GameObject("Inventory").AddComponent<Inventory>();
                }
            }
            return instance;
        }
    }

    #endregion Singleton Pattern

    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public List<InventoryEquipment> inventoryEquipments = new List<InventoryEquipment>();

    public List<string> itemsVisual = new List<string>();
    public List<string> equipmentVisual = new List<string>();
    uint maxItemAmount = 99;
    //public List<int> itemsAmounts = new List<int>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    void Awake()
    {
        if (instance != null) Destroy(this);

        DontDestroyOnLoad(gameObject);//le GameObject qui porte ce script ne sera pas détruit
    }

    public bool Add(Item item, uint quantityToAdd = 1)
    {
        if (!item.isDefaultItem)
        {

            if (item.isStackable)
            {
                bool itemAlreadyInInventory = false;
                foreach (InventoryItem inventoryItem in inventoryItems)
                {
                    if (inventoryItem.Item.name == item.name)
                    {
                        if (inventoryItem.Amount + quantityToAdd > maxItemAmount)
                        {
                            Debug.Log("You can't add anymore " + item.name + "s to inventory.");
                            UpdateEquipmentList();
                            return false;
                        }
                        inventoryItem.Amount += quantityToAdd;
                        Debug.Log("You have now " + inventoryItem.Amount + " " + inventoryItem.Item.name + "s in inventory.");
                        itemAlreadyInInventory = true;
                    }
                }

                if (!itemAlreadyInInventory)
                {
                    Debug.Log(item.name + " x" + quantityToAdd + " added to inventory.");
                    InventoryItem newInventoryItem = new InventoryItem(item, quantityToAdd);
                    inventoryItems.Add(newInventoryItem);
                }
            }
            else
            {
                Debug.Log(item.name + " x" + quantityToAdd + " added to inventory.");
                for (int i = 0; i < quantityToAdd; i++)
                {

                    InventoryItem newInventoryItem = new InventoryItem(item, 1);
                    inventoryItems.Add(newInventoryItem);
                }
            }
            onItemChangedCallback?.Invoke();
        }
        itemsVisual.Clear();
        foreach (InventoryItem invenItem in inventoryItems)
            itemsVisual.Add(invenItem.Item.name);
        UpdateEquipmentList();
        return true;
    }

    public void Remove(Item item, uint quantityToRemove = 1)
    {
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            if (inventoryItem.Item == item)
            {
                if (inventoryItem.Amount > quantityToRemove)
                {
                    inventoryItem.Amount -= quantityToRemove;
                    onItemChangedCallback?.Invoke();
                    UpdateEquipmentList();
                    return;
                }
                else
                {
                    inventoryItems.Remove(inventoryItem);
                    onItemChangedCallback?.Invoke();
                    UpdateEquipmentList();
                    return;
                }
            }
        }
    }

    public void RemoveEquipment(Equipment equipment, uint quantityToRemove = 1)
    {
        foreach (InventoryEquipment inventoryEquipment in inventoryEquipments)
        {
            if (inventoryEquipment.Equipment == equipment)
            {
                if (inventoryEquipment.Amount > quantityToRemove)
                {
                    inventoryEquipment.Amount -= quantityToRemove;
                    onItemChangedCallback?.Invoke();
                    return;
                }
                else
                {
                    Debug.Log(inventoryEquipments.Count);
                    inventoryEquipments.Remove(inventoryEquipment);
                    Debug.Log(inventoryEquipments.Count);
                    onItemChangedCallback?.Invoke();
                    return;
                    Debug.LogError("Error, did not succed to remove item");
                }
            }
        }
    }

    public void UpdateEquipmentList()
    {
        inventoryEquipments.Clear();
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            if (inventoryItem.Item is Equipment equipment)
            {
                InventoryEquipment newInventoryEquipment = new InventoryEquipment(equipment, 1);
                inventoryEquipments.Add(newInventoryEquipment);
            }
        }


        equipmentVisual.Clear();
        foreach (InventoryEquipment invenItem in inventoryEquipments)
            equipmentVisual.Add(invenItem.Equipment.name);
    }
}
