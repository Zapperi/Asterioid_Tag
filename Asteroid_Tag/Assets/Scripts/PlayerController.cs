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
    public Rigidbody playerShipRB;

    [SerializeField]
    private GameObject playerShip;

    public bool playerMovementReady;
    // Start is called before the first frame update

    private void Start()
    {
        GameController.instance.players.Add(this);       
        StartCoroutine("WaitForShipCreation");
    }

    /// <summary>
    /// Gets reference to the player's ship GameObject.
    /// </summary>
    /// <returns>Player's ship gameobject.</returns>
    public GameObject GetPlayerShip()
    {
        return playerShip;
    }

    private IEnumerator WaitForShipCreation()
    {
        // Instantiate a ship by given ship prefab at player's spawn location.
        playerShip = Instantiate(playerShipType, spawnLocation.position, spawnLocation.rotation, transform);
        // Cache reference to the owner into the created ship.
        playerShip.GetComponent<ShipController>().InitShip(this);
        // Cache reference of the player's ship's Rigidbody.
        playerShipRB = playerShip.GetComponent<Rigidbody>();
        // Enable PlayerMovement script.
        playerMovement.enabled = true;
        // Initialize player movement for this new ship.
        playerMovement.InitializePlayerMovement(playerShip, playerShipRB, playerID);
        // Wait until PlayerMovement has been set up. PlayerMovement script changes this boolean.
        while (!playerMovementReady)
        {
            yield return new WaitForSeconds(0.1f);
        }
        // Send event that a Ship has been created.
        GameController.instance.ShipCreated(playerID);
    }
}
