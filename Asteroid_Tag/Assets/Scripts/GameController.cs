using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public List<PlayerController> players;
    public bool loadIsReady;
    public Camera cam;
    public float screenWidth;
    public float screenHeight;

    // Start is called before the first frame update
    private void Awake()
    {
        // Make sure there is only one instance of GameController running.
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }

    }

    #region ---Events---

    public event Action<int> OnShipCreated;
    /// <summary>
    /// Send an event when ship is created.
    /// </summary>
    /// <param name="playerID">ID of the player that own's the ship.</param>
    public void ShipCreated(int playerID)
    {
        if(OnShipCreated != null)
        {
            OnShipCreated(playerID);
        }
    }

    /// <summary>
    /// Called whenever a player performs a Dash action. Requires playerID.
    /// </summary>
    public event Action<int> OnPlayerDashStart;
    /// <summary>
    /// Send an event when player starts a Dash action.
    /// </summary>
    /// <param name="playerID">ID of the player who is dashing.</param>
    public void PlayerDashStart(int playerID)
    {
        if(OnPlayerDashStart != null)
        {
            OnPlayerDashStart(playerID);
        }
    }
    /// <summary>
    /// Called whenever a player's Dash action ends. Requires playerID.
    /// </summary>
    public event Action<int> OnPlayerDashEnd;
    /// <summary>
    /// Send an event when player ends a Dash action.
    /// </summary>
    /// <param name="playerID">ID of the player who's dash ends.</param>
    public void PlayerDashEnd(int playerID)
    {
        if (OnPlayerDashEnd != null)
        {
            OnPlayerDashEnd(playerID);
        }
    }

    #endregion

    private void Start()
    {
        // Cache components and references.
        cam = Camera.main;
        // Check and calculate if we are using ortographic camera.
        if (cam.orthographic)
        {
            screenWidth = cam.orthographicSize * 2f * Screen.width / Screen.height;
            screenHeight =cam.orthographicSize * 2f;
        }
        else
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
        }
    }
}
