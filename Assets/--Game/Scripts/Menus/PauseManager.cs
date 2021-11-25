using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public enum State
    {
        PAUSE,
        ITEMS,
        SKILLS,
        EQUIPMENT,
        GEMPOD,
        STRATEGY,
        GUIDE,
        SYSTEM
    }
    public State currentState = State.PAUSE;

    public Player _mainPlayer;

    public int playerID;

    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject gameUI;
    [SerializeField] Animator menuAnimator;

    [Header("MainPause")]
    [SerializeField] TextMeshProUGUI categoryTitle;
    [SerializeField] CategoryButton[] pauseMenuCategories;

    [Header("ItemsMenu")]
    [SerializeField] CategoryButton[] itemsMenuCategories;

    bool joystickHorizontalPushed = false;
    bool joystickVerticalPushed = false;
    int categoryCursor = 0;

    // Start is called before the first frame update
    void Start()
    {
        _mainPlayer = ReInput.players.GetPlayer(playerID);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.gamePlaying) return;

        if (!GameManager.Instance.gamePaused)
        {
            if (_mainPlayer.GetButtonDown("Pause"))
            {
                OpenPause();
            }
        }
        else
        {
            if (_mainPlayer.GetButtonDown("Pause"))
            {
                if (currentState == State.PAUSE)
                {
                    ClosePause();
                }
            }

            if (_mainPlayer.GetButtonDown("MenuCancel"))
            {
                if (currentState == State.ITEMS)
                {
                    ReturnToMenuPause();
                }
                else if (currentState == State.PAUSE)
                {
                    ClosePause();
                }
            }

            if (_mainPlayer.GetButtonDown("MenuValidate"))
            {
                if (currentState == State.PAUSE)
                {
                    if (categoryCursor == 0)
                        GoToItems();
                }
            }

            if (_mainPlayer.GetAxis("MenuMoveAxisX") > 0.5f && !joystickHorizontalPushed)
            {
                joystickHorizontalPushed = true;
                if (currentState == State.PAUSE)
                {
                    MoveToCategory(true, pauseMenuCategories);
                    UpdateMenuPauseCategoryTitle();
                }
                //else if (currentState == State.ITEMS)
                //{
                //    MoveToCategory(true, itemsMenuCategories);
                //}
            }
            else if (_mainPlayer.GetAxis("MenuMoveAxisX") < -0.5f && !joystickHorizontalPushed)
            {
                joystickHorizontalPushed = true;
                if (currentState == State.PAUSE)
                {
                    MoveToCategory(false, pauseMenuCategories);
                    UpdateMenuPauseCategoryTitle();
                }
                //else if (currentState == State.ITEMS)
                //{
                //    MoveToCategory(false, itemsMenuCategories);
                //}
            }

            if (_mainPlayer.GetButtonDown("RightBumper"))
            {
                if (currentState == State.ITEMS)
                {
                    MoveToCategory(true, itemsMenuCategories);
                }
            }

            if (_mainPlayer.GetButtonDown("LeftBumper"))
            {
                if (currentState == State.ITEMS)
                {
                    MoveToCategory(false, itemsMenuCategories);
                }
            }

            if (Mathf.Abs(_mainPlayer.GetAxis("MenuMoveAxisX")) < 0.5f && joystickHorizontalPushed)
            {
                joystickHorizontalPushed = false;
            }
            /*if (_mainPlayer.GetButtonDown("Pause"))
            {
                if (!GameManager.Instance.gamePaused)
                {
                    OpenPause();
                }
                else
                {
                    if (currentState == State.PAUSE)
                        ClosePause();
                }
            }*/
        }
    }

    private void ClosePause()
    {
        gameUI.SetActive(true);
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.gamePaused = false;
    }

    void OpenPause()
    {
        foreach (CategoryButton category in pauseMenuCategories)
            category.UnSelect();


        categoryCursor = 0;
        pauseMenuCategories[categoryCursor].Select();
        UpdateMenuPauseCategoryTitle();

        gameUI.SetActive(false);
        pauseCanvas.SetActive(true);
        currentState = State.PAUSE;

        Time.timeScale = 0f;
        GameManager.Instance.gamePaused = true;
    }

    void GoToItems()
    {
        menuAnimator.SetTrigger("Items");

        foreach (CategoryButton category in itemsMenuCategories)
            category.UnSelect();

        categoryCursor = 0;
        itemsMenuCategories[categoryCursor].Select();
        currentState = State.ITEMS;
    }

    void ReturnToMenuPause()
    {
        menuAnimator.SetTrigger("MainMenu");

        foreach (CategoryButton category in pauseMenuCategories)
            category.UnSelect();

        categoryCursor = 0;
        pauseMenuCategories[categoryCursor].Select();
        currentState = State.PAUSE;
    }
    void MoveToCategory(bool moveToNextCategory, CategoryButton[] categories)
    {
        categories[categoryCursor].UnSelect();
        if (moveToNextCategory)
        {
            if (categoryCursor < categories.Length - 1)
                categoryCursor++;
            else
                categoryCursor = 0;
        }
        else
        {
            if (categoryCursor > 0)
                categoryCursor--;
            else
                categoryCursor = categories.Length - 1;
        }
        categories[categoryCursor].Select();
    }

    void UpdateMenuPauseCategoryTitle()
    {
        switch (categoryCursor)
        {
            case 0:
                categoryTitle.text = "Items";
                break;
            case 1:
                categoryTitle.text = "Skills";
                break;
            case 2:
                categoryTitle.text = "Equipment";
                break;
            case 3:
                categoryTitle.text = "Gem Pod";
                break;
            case 4:
                categoryTitle.text = "Strategy";
                break;
            case 5:
                categoryTitle.text = "Guide";
                break;
            case 6:
                categoryTitle.text = "System";
                break;
        }
    }
}
