using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EquipmentSlot : MonoBehaviour
{
    public TextMeshProUGUI equipmentNameText;

    public EquipmentType type;

    public bool isFirstAccessory = false;

    [HideInInspector] public bool selected = false;

    public void UpdateText(string text)
    {
        equipmentNameText.text = text;
    }
}
