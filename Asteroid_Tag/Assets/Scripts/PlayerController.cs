using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerID;
    public int playerLives;
    public Color playerColor;
    public GameObject playerShipType;
    public PlayerMovement playerMovement;
    public Transform spawnLocation;
    public ShipController playerShip;
    public bool playerDashing;
    public bool dashIsReady;
    public float dashCooldown;
    public float dashCooldownTimer;

    public bool playerMovementReady;
    // Start is called before the first frame update

    private void Start()
    {
        GameController.instance.OnPlayerDashStart += PlayerDashStart;
        GameController.instance.OnPlayerDashEnd += PlayerDashEnd;
        GameController.instance.players.Add(this);       
        StartCoroutine("WaitForShipCreation");
    }

    /// <summary>
    /// Gets reference to the player's ship GameObject.
    /// </summary>
    /// <returns>Player's ship gameobject.</returns>
    public GameObject GetPlayerShip()
    {
        return playerShip.gameObject;
    }

    private IEnumerator WaitForShipCreation()
    {
        GameObject tempShip;
        // Instantiate a ship by given ship prefab at player's spawn location.
        tempShip = Instantiate(playerShipType, spawnLocation.position, spawnLocation.rotation, transform);
        // Cache reference to the owner into the created ship.
        playerShip = tempShip.GetComponent<ShipController>().InitShip(this);
        // Enable PlayerMovement script.
        playerMovement.enabled = true;
        // Initialize player movement for this new ship.
        playerMovement.InitializePlayerMovement(playerShip, playerID);
        // Wait until PlayerMovement has been set up. PlayerMovement script changes this boolean.
        while (!playerMovementReady)
        {
            yield return new WaitForSeconds(0.1f);
        }
        // Send event that a Ship has been created.
        GameController.instance.ShipCreated(playerID);
    }

    private void PlayerDashStart(int id)
    {
        if(id == playerID)
        {
            dashIsReady = false;
            playerDashing = true;
            playerMovement.enabled = false;
        }
    }

    private void PlayerDashEnd(int id)
    {
        if(id == playerID)
        {
            playerDashing = false;
            playerMovement.enabled = true;
            StartCoroutine("TriggerDashCooldown");
        }
    }

    IEnumerator TriggerDashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        dashIsReady = true;
    }

    private void OnDestroy()
    {
        GameController.instance.OnPlayerDashStart -= PlayerDashStart;
        GameController.instance.OnPlayerDashEnd -= PlayerDashEnd;
    }
}
