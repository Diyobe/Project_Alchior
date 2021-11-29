using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;
using System;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] GameObject equipmentInventory;
    [SerializeField] Transform slotsPosition;
    [SerializeField] Transform equipmentListPosition;
    [Space]
    [SerializeField] EquipmentSlot[] equipmentSlots;
    [SerializeField] string emptySlotText = "---";
    int equipmentSlotCursor = 0;
    int inventoryCursor = 0;
    bool joystickVerticalPushed = false;
    Player inputPlayer;

    [SerializeField] GameObject inventoryElementPrefab;

    [SerializeField] RectTransform inventoryElementRectTransform;
    int indexLimit = 0;
    [SerializeField] int scrollSize = 3;
    IEnumerator coroutineScroll = null;
    [SerializeField] RectTransform content;
    [SerializeField] RectTransform UICursor;
    List<InventoryUIElement> inventoryUIElements = new List<InventoryUIElement>();

    [Space]
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] TextMeshProUGUI maxHPStatText;
    [SerializeField] TextMeshProUGUI maxTHStatText;
    [SerializeField] TextMeshProUGUI attackStatText;
    [SerializeField] TextMeshProUGUI specialAttackStatText;
    [SerializeField] TextMeshProUGUI defenseStatText;
    [SerializeField] TextMeshProUGUI specialDefenseStatText;
    [SerializeField] Transform characterModelParent;

    [SerializeField] PauseManager pauseManager;
    PartyManager partyManager;

    int characterCursor = 0;

    bool isSlotSelected = false;

    private void OnEnable()
    {
        partyManager = PartyManager.Instance;
        isSlotSelected = false;
        equipmentInventory.SetActive(false);
        equipmentSlotCursor = 0;
        inventoryCursor = 0;

        UpdateEquipmentSlots(partyManager.characterDatas[0]);
        UpdateStatTexts(partyManager.characterDatas[0]);
        UpdateCharacterModel(partyManager.characterDatas[0]);
        StartCoroutine(DelayedUpdateCursorPosition());
    }

    private void UpdateCharacterModel(CharacterData characterData)
    {
        foreach (Transform child in characterModelParent)
            Destroy(child.gameObject);

        Instantiate(characterData.characterMenuModel, characterModelParent);
    }

    void Start()
    {
        inputPlayer = ReInput.players.GetPlayer(0);

        UpdateEquipmentSlots(PartyManager.Instance.characterDatas[0]);
        UpdateStatTexts(PartyManager.Instance.characterDatas[0]);
    }

    private void Update()
    {
        if (!GameManager.Instance.gamePlaying || !GameManager.Instance.gamePaused || GameManager.Instance.isInCutscene || GameManager.Instance.popUpOpened) return;
        if (pauseManager.currentState != PauseManager.State.EQUIPMENT) return;

        if (inputPlayer.GetButtonDown("MenuValidate"))
        {
            if (!isSlotSelected)
                OpenEquipmentInventory();
            else
            {
                if (inventoryUIElements.Count <= 0) return;
                partyManager.EquipItem(partyManager.characterDatas[characterCursor]
                                              , inventoryUIElements[inventoryCursor].currentInventoryEquipment.Equipment
                                              , equipmentSlots[equipmentSlotCursor].isFirstAccessory);

                UpdateEquipmentSlots(partyManager.characterDatas[characterCursor]);
                UpdateStatTexts(partyManager.characterDatas[characterCursor]);
                CloseEquipmentInventory();
            }
        }

        if (inputPlayer.GetButtonDown("MenuCancel"))
        {
            if (!isSlotSelected)
                pauseManager.ReturnToMenuPause();
            else
                CloseEquipmentInventory();
        }

        if (inputPlayer.GetButtonDown("Toss/Unequip"))
        {
            partyManager.Unequip(partyManager.characterDatas[characterCursor],
                                            ReturnSlotEquipment(equipmentSlots[equipmentSlotCursor], partyManager.characterDatas[characterCursor]),
                                            equipmentSlots[equipmentSlotCursor].isFirstAccessory);
            UpdateSlotText(equipmentSlots[equipmentSlotCursor], partyManager.characterDatas[characterCursor]);
            UpdateStatTexts(partyManager.characterDatas[characterCursor]);
        }

        if (inputPlayer.GetAxis("MenuMoveAxisY") > 0.5f && !joystickVerticalPushed)
        {
            joystickVerticalPushed = true;
            MoveToElement(false);
        }
        else if (inputPlayer.GetAxis("MenuMoveAxisY") < -0.5f && !joystickVerticalPushed)
        {
            joystickVerticalPushed = true;
            MoveToElement(true);
            //else if (currentState == State.ITEMS)
            //{
            //    MoveToCategory(false, itemsMenuCategories);
            //}
        }

        if (Mathf.Abs(inputPlayer.GetAxis("MenuMoveAxisY")) < 0.5f && joystickVerticalPushed)
        {
            joystickVerticalPushed = false;
        }
    }

    void MoveToElement(bool moveToNextElement)
    {
        if (isSlotSelected)
        {
            if (inventoryUIElements.Count <= 0) return;

            inventoryUIElements[inventoryCursor].UnSelect();
            if (moveToNextElement)
            {
                if (inventoryCursor < inventoryUIElements.Count - 1)
                    inventoryCursor++;
                else
                    inventoryCursor = 0;
            }
            else
            {
                if (inventoryCursor > 0)
                    inventoryCursor--;
                else
                    inventoryCursor = inventoryUIElements.Count - 1;
            }
            inventoryUIElements[inventoryCursor].Select();
        }
        else
        {
            equipmentSlots[equipmentSlotCursor].selected = false;
            if (moveToNextElement)
            {
                if (equipmentSlotCursor < equipmentSlots.Length - 1)
                    equipmentSlotCursor++;
                else
                    equipmentSlotCursor = 0;
            }
            else
            {
                if (equipmentSlotCursor > 0)
                    equipmentSlotCursor--;
                else
                    equipmentSlotCursor = equipmentSlots.Length - 1;
            }
            equipmentSlots[equipmentSlotCursor].selected = true;
        }
        MoveScrollRect();
        StartCoroutine(DelayedUpdateCursorPosition());
    }

    private void UpdateCursorPosition()
    {
        if (isSlotSelected)
        {
            //Debug.Log(equipmentSlotCursor);
            if (inventoryUIElements.Count <= 0) return;
            //Debug.Log("Cursor Position Updated");

            UICursor.position = new Vector3(equipmentListPosition.position.x,
                                            inventoryUIElements[inventoryCursor].GetComponent<RectTransform>().position.y,
                                            UICursor.position.z);
        }
        else
        {
            UICursor.position = new Vector3(slotsPosition.position.x,
                                            equipmentSlots[equipmentSlotCursor].GetComponent<RectTransform>().position.y,
                                            UICursor.position.z);
        }
    }

    void UpdateEquipmentSlots(CharacterData character)
    {
        if (character == null) return;

        foreach (EquipmentSlot slot in equipmentSlots)
        {
            UpdateSlotText(slot, character);
        }
    }

    void UpdateSlotText(EquipmentSlot slot, CharacterData character)
    {
        if (slot.type == EquipmentType.WEAPON)
        {
            slot.UpdateText(character.weapon ? character.weapon.name : emptySlotText);
        }
        else if (slot.type == EquipmentType.CHEST)
        {
            slot.UpdateText(character.chestEquipment ? character.chestEquipment.name : emptySlotText);
        }
        else if (slot.type == EquipmentType.LEGS)
        {
            slot.UpdateText(character.legsEquipment ? character.legsEquipment.name : emptySlotText);
        }
        else
        {
            if (slot.isFirstAccessory)
                slot.UpdateText(character.firstAccessory ? character.firstAccessory.name : emptySlotText);
            else
                slot.UpdateText(character.secondAccessory ? character.secondAccessory.name : emptySlotText);

        }
    }

    Equipment ReturnSlotEquipment(EquipmentSlot slot, CharacterData character)
    {
        if (slot.type == EquipmentType.WEAPON)
            return character.weapon;
        else if (slot.type == EquipmentType.CHEST)
            return character.chestEquipment;
        else if (slot.type == EquipmentType.LEGS)
            return character.legsEquipment;
        else
        {
            if (slot.isFirstAccessory)
                return character.firstAccessory;
            else
                return character.secondAccessory;

        }
    }

    public void OpenEquipmentInventory()
    {
        equipmentInventory.SetActive(true);
        isSlotSelected = true;
        inventoryCursor = 0;


        List<InventoryEquipment> equipments = new List<InventoryEquipment>(Inventory.Instance.inventoryEquipments);

        foreach (Transform child in content)
            Destroy(child.gameObject);

        inventoryUIElements.Clear();
        if (Inventory.Instance.inventoryEquipments.Count == 0) return;

        foreach (InventoryEquipment inventoryEquipment in equipments)
        {
            if (inventoryEquipment.Equipment.equipmentType == equipmentSlots[equipmentSlotCursor].type)
            {
                InventoryUIElement inventoryUIElement = Instantiate(inventoryElementPrefab, content).GetComponent<InventoryUIElement>();
                inventoryUIElement.UpdateElementVisualEquipment(inventoryEquipment);
                inventoryUIElements.Add(inventoryUIElement);
            }
        }

        if (inventoryUIElements.Count == 0) return;
        foreach (InventoryUIElement element in inventoryUIElements)
            element.UnSelect();
        inventoryUIElements[inventoryCursor].Select();
        StartCoroutine(DelayedUpdateCursorPosition());
    }

    public void CloseEquipmentInventory()
    {
        isSlotSelected = false;
        StartCoroutine(DelayedUpdateCursorPosition());
        equipmentInventory.SetActive(false);
    }

    void UpdateStatTexts(CharacterData character)
    {
        characterNameText.text = character.characterName;
        maxHPStatText.text = character.GetMaxHP().ToString();
        maxTHStatText.text = character.GetMaxTH().ToString();
        attackStatText.text = character.GetAttack().ToString();
        specialAttackStatText.text = character.GetSpecialAttack().ToString();
        defenseStatText.text = character.GetDefense().ToString();
        specialDefenseStatText.text = character.GetSpecialDefense().ToString();
    }

    IEnumerator DelayedUpdateCursorPosition()
    {
        yield return new WaitForSecondsRealtime(0.0001f);
        UpdateCursorPosition();
    }

    protected void MoveScrollRect()
    {
        if (content == null) return;

        if (inventoryCursor > indexLimit)
        {
            indexLimit = inventoryCursor;
            coroutineScroll = MoveScrollRectCoroutine();
            if (coroutineScroll != null)
            {
                StopCoroutine(coroutineScroll);
            }
            StartCoroutine(coroutineScroll);
        }
        else if (inventoryCursor < indexLimit - scrollSize + 1)
        {
            indexLimit = inventoryCursor + scrollSize - 1;
            coroutineScroll = MoveScrollRectCoroutine();
            if (coroutineScroll != null)
            {
                StopCoroutine(coroutineScroll);
            }
            StartCoroutine(coroutineScroll);
        }

    }

    private IEnumerator MoveScrollRectCoroutine()
    {
        //float t = 0f;
        //float speed = 1 / 0.1f;
        int ratio = indexLimit - scrollSize;
        Vector2 destination = new Vector2(0, Mathf.Clamp(ratio * inventoryElementRectTransform.sizeDelta.y, 0, (inventoryUIElements.Count - scrollSize) * inventoryElementRectTransform.sizeDelta.y));

        content.anchoredPosition = destination;

        yield return null;
    }
}
