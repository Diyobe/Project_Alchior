using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rewired;

public class UIPopUp : MonoBehaviour
{
    public enum Type
    {
        ItemUseValidation,
        ItemTossValidation,
        ItemTossAmount,
        ItemUseOnPartyMember,
    }

    public Type type;

    [HideInInspector]
    public bool isValidation = true;
    InventoryUIElement[] validationButtons;
    [SerializeField] InventoryUIElement okButton;
    [SerializeField] Slider sliderAmount;
    [SerializeField] TextMeshProUGUI validationTextMeshPro;
    Player inputPlayer;
    bool joystickVerticalPushed = false;

    //[HideInInspector] public string popUpMessage;
    private void Awake()
    {
        inputPlayer = ReInput.players.GetPlayer(0);
    }

    private void Update()
    {
        if (Mathf.Abs(inputPlayer.GetAxis("MenuMoveAxisY")) > 0.5f && !joystickVerticalPushed)
        {
            joystickVerticalPushed = true;
            isValidation = !isValidation;
            ChangeValidationCursor();
        }
        if (Mathf.Abs(inputPlayer.GetAxis("MenuMoveAxisY")) < 0.5f && joystickVerticalPushed)
        {
            joystickVerticalPushed = false;
        }
    }

    private void OnEnable()
    {
        if (type == Type.ItemUseValidation)
        {
            isValidation = true;
            ChangeValidationCursor();
        }
    }

    public void ChangeValidationCursor()
    {
        if (isValidation)
        {
            validationButtons[0].Select();
            validationButtons[1].UnSelect();
        }
        else
        {
            validationButtons[1].Select();
            validationButtons[0].UnSelect();
        }
    }
}
