using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public PlayerController playerController;
    public ShipController shipToControl;

    public string playerInputString;

    void FixedUpdate()
    {
        if (Input.GetButtonDown(playerInputString + "Gas") && playerController.dashIsReady)
        {
            shipToControl.ShipDash();
        }
        // Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis(playerInputString + "Horizontal");
        // Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis(playerInputString + "GasAxis");
        
        // Turn the ship by horizontal input.
        if(moveHorizontal != 0f)
        {
            shipToControl.TurnShip(moveHorizontal);
        }

        // If there is input from player..
        if (moveVertical != 0f)
        {
            shipToControl.AddThrust(moveVertical);
        }
        
        // If dampeners are active and player is not moving, slow the ship down by using Drag.

        //else if (dampeners && playerRB.drag != 0.5f)
        //{
        //    playerRB.drag = 0.5f;
        //}
    }

    /// <summary>
    /// Prepares the PlayerMovement script.
    /// </summary>
    /// <param name="ship">Ship to control.</param>
    /// <param name="rb">Ship's RigidBody.</param>
    /// <param name="id">Player ID.</param>
    public void InitializePlayerMovement(ShipController ship, int id)
    {
        shipToControl = ship;
        playerInputString = "Player" + id;
        // Tell the controller that this PlayerMovement script is ready.
        playerController.playerMovementReady = true;
    }
}
