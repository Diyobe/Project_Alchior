using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Sirenix.OdinInspector;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    CharacterState currentState;

    [SerializeField]
    CharacterState idleState;

    [SerializeField]
    CharacterState aerialState;
    [SerializeField]
    CharacterState landingState;

    public CharacterState CurrentState
    {
        get { return currentState; }
    }

    [SerializeField]
    private CharacterData characterData;
    public CharacterData CharacterData
    {
        get { return characterData; }
    }

    [Title("Model")]
    [SerializeField]
    private Transform centerPoint;
    public Transform CenterPoint
    {
        get { return centerPoint; }
    }
    [SerializeField]
    private Transform centerPivot;
    public Transform CenterPivot
    {
        get { return centerPivot; }
    }

    [SerializeField]
    private GameObject model;
    public GameObject Model
    {
        get { return model; }
    }

    [Title("Components")]
    [SerializeField]
    private CharacterRigibody rigidbody;
    public CharacterRigibody Rigidbody
    {
        get { return rigidbody; }
    }

    [SerializeField]
    private CharacterMovement movement;
    public CharacterMovement Movement
    {
        get { return movement; }
    }

    [SerializeField]
    private CharacterAction action;
    public CharacterAction Action
    {
        get { return action; }
    }

    [SerializeField]
    private CharacterKnockback knockback;
    public CharacterKnockback Knockback
    {
        get { return knockback; }
    }

    [SerializeField]
    private CharacterStats stats;
    public CharacterStats Stats
    {
        get { return stats; }
    }

    [SerializeField]
    private Sprite characterIcon;
    public Sprite CharacterIcon
    {
        get { return characterIcon; }
    }

    public delegate void ActionFloat(float value);
    public event ActionFloat OnMotionSpeed;

    [Title("Parameter")]
    [SerializeField]
    [ReadOnly]
    private float motionSpeed = 1;
    public float MotionSpeed
    {
        get { return motionSpeed; }
    }
    private IEnumerator motionSpeedCoroutine;

    public PlayerMainActions actions;

    public Player inputPlayer;
    public bool canLand = true;

    public delegate void ActionSetState(CharacterState oldState, CharacterState newState);
    public event ActionSetState OnStateChanged;


    [SerializeField] int playerID = 0;
    private void Start()
    {
        if (playerID == 0)
            inputPlayer = ReInput.players.GetPlayer(playerID);
        else
            inputPlayer = ReInput.players.SystemPlayer;

        SetState(CurrentState);
        action.InitializeComponent(this);
    }

    public void SetState(CharacterState characterState)
    {
        //Debug.Log(characterState.gameObject.name);
        if (currentState != null)
            currentState.EndState(this, characterState);

        CharacterState oldState = currentState;
        currentState = characterState;

        currentState.StartState(this, oldState);

        OnStateChanged?.Invoke(oldState, currentState);
    }

    private void Update()
    {
        if (GameManager.Instance.gamePaused || GameManager.Instance.isInCutscene || GameManager.Instance.popUpOpened) return;
        action.CanEndAction();

        //knockback.CheckHit(this);

        currentState.UpdateState(this);
        currentState.LateUpdateState(this);

        action.EndActionState();

    }

    public void ResetToIdle()
    {
        if (rigidbody.IsGrounded())
        {
            SetState(idleState);
        }
        else
        {
            ResetToAerial();
        }
    }

    public void ResetToAerial()
    {
        SetState(aerialState);
    }

    public void ResetToLand()
    {
        SetState(landingState);
    }

    public void SetMotionSpeed(float newValue, float time)
    {
        motionSpeed = newValue;
        Movement.MotionSpeed = MotionSpeed;
        Knockback.MotionSpeed = MotionSpeed;
        Action.SetAttackMotionSpeed(MotionSpeed);

        if (motionSpeedCoroutine != null)
            StopCoroutine(motionSpeedCoroutine);
        motionSpeedCoroutine = MotionSpeedCoroutine(time);
        StartCoroutine(motionSpeedCoroutine);
    }

    private IEnumerator MotionSpeedCoroutine(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        motionSpeed = 1;
        Movement.MotionSpeed = MotionSpeed;
        Knockback.MotionSpeed = MotionSpeed;
        Action.SetAttackMotionSpeed(MotionSpeed);
    }
}
