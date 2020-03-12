using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public Rigidbody playerRB;
    public PlayerController playerController;
    public GameObject shipToControl;

    public bool dampeners = false;
    [Range(1,10)]
    public float acceleration;
    [Range(1, 10)]
    public float turningSpeed;
    public float maxSpeed;
    public float drag = 0f;
    public float currentSpeed;

    private float accelerationModifier;
    private float maxAcceleration = 60f;
    private float rotationSpeedModifier;
    private float maxRotationSpeed = 15f;
    private string playerInputString;

    private void Start()
    {
        // Increase the ranged values to higher values.
        accelerationModifier = (acceleration / 10) * maxAcceleration;
        rotationSpeedModifier = (turningSpeed / 10) * maxRotationSpeed;
    }
    private void Update()
    {
        if (Input.GetButton(playerInputString+"Gas"))
        {
            // TODO; Change this to event.
            dampeners = !dampeners;
        }
    }

    void FixedUpdate()
    {
        // Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis(playerInputString + "Horizontal");
        // Turn the ship by horizontal input.
        playerRB.AddTorque(0f, moveHorizontal * rotationSpeedModifier * Time.fixedDeltaTime, 0f, ForceMode.VelocityChange);

        // Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis(playerInputString + "GasAxis");

        // Update current speed from rigibody.
        currentSpeed = playerRB.velocity.magnitude;

        // If there is input from player..
        if (moveVertical != 0f)
        {
            playerRB.drag = 0f;
            // Calculate the magnitude of speed after new addinational force input.
            float newSpeed = (playerRB.velocity + (shipToControl.transform.forward * moveVertical * accelerationModifier * Time.fixedDeltaTime)).magnitude;

            // If new speed over max speed AND over current speed, do not increase our speed. 
            if ((newSpeed > maxSpeed) && newSpeed >= currentSpeed)
            {
                //TODO; Add call to maxspeed effect.
            }
            // If speed is under max speed or going lower if we are over max speed, apply the new speed using VelocityChange (ignores mass, instant change).
            else
            {
                playerRB.AddForce(shipToControl.transform.forward * moveVertical * accelerationModifier * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
        // If dampeners are active and player is not moving, slow the ship down by using Drag.
        else if (dampeners && playerRB.drag != 0.5f)
        {
            playerRB.drag = 0.5f;
        }
    }

    /// <summary>
    /// Prepares the PlayerMovement script.
    /// </summary>
    /// <param name="ship">Ship to control.</param>
    /// <param name="rb">Ship's RigidBody.</param>
    /// <param name="id">Player ID.</param>
    public void InitializePlayerMovement(GameObject ship, Rigidbody rb, int id)
    {
        shipToControl = ship;
        playerRB = rb;
        playerInputString = "Player" + id;
        // Tell the controller that this PlayerMovement script is ready.
        playerController.playerMovementReady = true;
    }

    //private void OnDestroy()
    //{
    //    #region ---Unsubscripe Events---

       

    //    #endregion
    //}
}
