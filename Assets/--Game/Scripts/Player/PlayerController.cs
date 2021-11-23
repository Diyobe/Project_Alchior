using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{

    //Clarence Berard

    public PlayerRigidBodyEntity _playerRigidBody;
    public PlayerMainActions _playerActions;

    public Player _mainPlayer;

    public int playerID;

    // Start is called before the first frame update
    void Start()
    {
        _mainPlayer = ReInput.players.GetPlayer(playerID);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.gamePlaying || GameManager.Instance.gamePaused) return;

        float dirX = _mainPlayer.GetAxis("MoveX");
        float dirZ = _mainPlayer.GetAxis("MoveZ");

        _playerRigidBody.Move(dirX, dirZ);

        if (_mainPlayer.GetButtonDown("Jump"))
        {
            _playerRigidBody.Jump();
        }

        if (_mainPlayer.GetButtonDown("Interact"))
        {
            _playerActions.Interact();
        }

        if (_playerRigidBody.verticalSpeed == 0)
            _playerRigidBody.isRunningInputPressed = _mainPlayer.GetButton("Run");
    }
}
