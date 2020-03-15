using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Renderer cockpitRenderer;
    public PlayerController player;
    public Rigidbody rb;

    public bool dampeners = false;
    [Range(1, 10)]
    public float accelerationModifier;
    [Range(1, 10)]
    public float turningSpeedModifier;
    [Range(1,10)]
    public float topSpeedModifier;
    [Range(1, 10)]
    public float shipMassModifier;
    public float currentSpeed;

    //private float shipDrag = 0f;
    private float dashPower = 10000f;
    private float dashLenght = 0.25f;
    private float acceleration;
    private float maxAcceleration = 60f;
    private float torque;
    private float maxRotationSpeed = 15f;
    private float topSpeed;
    private float maxTopSpeed = 100f;
    private float shipMass;
    private float maxShipMass = 1000f;

    private string playerInputString;
    // Start is called before the first frame update
    void Start()
    {        
        cockpitRenderer.material.SetColor("_BaseColor", GameController.instance.players[player.playerID-1].playerColor);
        acceleration = (accelerationModifier / 10) * maxAcceleration;
        torque = (turningSpeedModifier / 10) * maxRotationSpeed;
        topSpeed = (topSpeedModifier / 10) * maxTopSpeed;
        shipMass = (shipMassModifier / 10) * maxShipMass;
        rb.mass = shipMass;
    }
    
    /// <summary>
    /// Sets the owner of the ship.
    /// </summary>
    /// <param name="owner">Player that owns the ship.</param>
    public ShipController InitShip(PlayerController owner)
    {
        player = owner;
        return this;
    }

    private void FixedUpdate()
    {
        currentSpeed = rb.velocity.magnitude;
    }

    /// <summary>
    /// Add thrust to a ship.
    /// </summary>
    /// <param name="moveAxis">Value of the forward movement axis.</param>
    public void AddThrust(float moveAxis)
    {
        // Calculate the magnitude of speed after new addinational force input.
        float newSpeed = (rb.velocity + (transform.forward * moveAxis * acceleration * Time.fixedDeltaTime)).magnitude;

        // If new speed over max speed AND over current speed, do not increase our speed. 
        if ((newSpeed > topSpeed) && newSpeed >= currentSpeed)
        {
            //TODO; Add call to maxspeed effect.
        }
        // If speed is under max speed or going lower if we are over max speed, apply the new speed using VelocityChange (ignores mass, instant change).
        else
        {
            rb.AddForce(transform.forward * moveAxis * acceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// Add torque to a ship.
    /// </summary>
    /// <param name="turnAxis">Value of rotation axis.</param>
    public void TurnShip(float turnAxis)
    {
        rb.AddTorque(0f, turnAxis * torque * Time.fixedDeltaTime, 0f, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Starts ship Dash action.
    /// </summary>
    public void ShipDash()
    {
        StartCoroutine("DoShipDash");
    }

    private IEnumerator DoShipDash()
    {
        GameController.instance.PlayerDashStart(player.playerID);
        Debug.Log("PLAYER: " + player.playerID + " DASH START!");
        float originalDrag = rb.drag;
        rb.mass = rb.mass * 10;
        rb.AddForce(transform.forward * dashPower * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.drag = 5f;
        yield return new WaitForSeconds(dashLenght);
        rb.drag = originalDrag;
        rb.mass = shipMass;
        Debug.Log("PLAYER: " + player.playerID + " DASH END!");
        GameController.instance.PlayerDashEnd(player.playerID);
    }
}
