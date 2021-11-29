using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIElement : MonoBehaviour
{
    [HideInInspector]
    public InventoryItem currentInventoryItem;
    [HideInInspector]
    public InventoryEquipment currentInventoryEquipment;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI elementNameText;
    [SerializeField] TextMeshProUGUI elementQuantityText;
    Animator animator;
    [SerializeField] UIPopUp itemUsePopUp;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateElementVisual(InventoryItem inventoryItem)
    {
        currentInventoryItem = inventoryItem;
        icon.sprite = currentInventoryItem.Item.icon;
        elementNameText.text = currentInventoryItem.Item.name;
        elementQuantityText.text = currentInventoryItem.Amount.ToString();
    }

    public void UpdateElementVisualEquipment(InventoryEquipment inventoryEquipment)
    {
        currentInventoryEquipment = inventoryEquipment;
        icon.sprite = currentInventoryEquipment.Equipment.icon;
        elementNameText.text = currentInventoryEquipment.Equipment.name;
        elementQuantityText.text = currentInventoryEquipment.Amount.ToString();
    }

    public void Select()
    {
        animator.SetBool("Selected", true);
    }

    public void UnSelect()
    {
        animator.SetBool("Selected", false);
    }
}
