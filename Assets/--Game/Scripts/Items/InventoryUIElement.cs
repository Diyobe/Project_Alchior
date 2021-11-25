using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIElement : MonoBehaviour
{
    InventoryItem currentInventoryItem;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI elementNameText;
    [SerializeField] TextMeshProUGUI elementQuantityText;
    Animator animator;

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

    public void Select()
    {
        animator.SetBool("Selected", true);
    }

    public void UnSelect()
    {
        animator.SetBool("Selected", false);
    }
}
