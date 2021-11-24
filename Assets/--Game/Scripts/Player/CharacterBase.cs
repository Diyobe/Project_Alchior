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


	public Player inputPlayer;
	public bool canLand = true;

	public delegate void ActionSetState(CharacterState oldState, CharacterState newState);
	public event ActionSetState OnStateChanged;

	private void Start()
	{
		inputPlayer = ReInput.players.GetPlayer(0);
		SetState(CurrentState);
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
		currentState.UpdateState(this);
		currentState.LateUpdateState(this);

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
}
