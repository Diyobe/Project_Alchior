using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    [SerializeField] TextMeshProUGUI elementDescriptionText;
    [SerializeField] Transform content;
    [SerializeField] GameObject inventoryElementPrefab;
    [SerializeField] RectTransform UICursor;
    List<InventoryUIElement> inventoryUIElements = new List<InventoryUIElement>();
    int cursorNumber = 0; //Le nom est à changer

    bool joystickVerticalPushed = false;
    Player inputPlayer;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.Instance;
        inputPlayer = ReInput.players.GetPlayer(0);


    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.gamePlaying || !GameManager.Instance.gamePaused || GameManager.Instance.isInCutscene || GameManager.Instance.popUpOpened || inventoryUIElements.Count <= 0) return;

        if (inputPlayer.GetButtonDown("MenuValidate"))
        {
            inventoryUIElements[cursorNumber].currentInventoryItem.Item.Use();
        }

        if (inputPlayer.GetAxis("MenuMoveAxisY") > 0.5f && !joystickVerticalPushed)
        {
            joystickVerticalPushed = true;
            MoveToElement(true);
        }
        else if (inputPlayer.GetAxis("MenuMoveAxisY") < -0.5f && !joystickVerticalPushed)
        {
            joystickVerticalPushed = true;
            MoveToElement(false);
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

    private void OnEnable()
    {
        cursorNumber = 0;
        UpdateUI();
        if (inventoryUIElements.Count > 0)
        {
            UICursor.gameObject.SetActive(true);
            foreach (InventoryUIElement element in inventoryUIElements)
                element.UnSelect();
            inventoryUIElements[cursorNumber].Select();
            StartCoroutine(DelayedUpdateCursorPosition());
        }
        else
        {
            UICursor.gameObject.SetActive(false);
        }
    }

    private void UpdateUI(List<InventoryItem> invetoryItems)
    {
        Debug.Log("Updating UI");
        foreach (Transform child in content)
            Destroy(child.gameObject);

        inventoryUIElements.Clear();
        if (invetoryItems.Count == 0) return;

        foreach (InventoryItem inventoryItem in invetoryItems)
        {
            InventoryUIElement inventoryUIElement = Instantiate(inventoryElementPrefab, content).GetComponent<InventoryUIElement>();
            inventoryUIElement.UpdateElementVisual(inventoryItem);
            inventoryUIElements.Add(inventoryUIElement);
        }
    }

    private void UpdateUI()
    {
        UpdateUI(Inventory.Instance.inventoryItems);
    }

    private void UpdateCursorPosition()
    {
        Debug.Log(cursorNumber);
        if (inventoryUIElements.Count <= 0) return;
        Debug.Log("Cursor Position Updated");

        UICursor.position = new Vector3(UICursor.position.x,
                                        inventoryUIElements[cursorNumber].GetComponent<RectTransform>().position.y,
                                        UICursor.position.z);
    }

    void MoveToElement(bool moveToNextElement)
    {
        inventoryUIElements[cursorNumber].UnSelect();
        if (moveToNextElement)
        {
            if (cursorNumber < inventoryUIElements.Count - 1)
                cursorNumber++;
            else
                cursorNumber = 0;
        }
        else
        {
            if (cursorNumber > 0)
                cursorNumber--;
            else
                cursorNumber = inventoryUIElements.Count - 1;
        }
        inventoryUIElements[cursorNumber].Select();
        UpdateCursorPosition();
    }

    IEnumerator DelayedUpdateCursorPosition()
    {
        yield return new WaitForSecondsRealtime(0.0001f);
        UpdateCursorPosition();
    }
}
