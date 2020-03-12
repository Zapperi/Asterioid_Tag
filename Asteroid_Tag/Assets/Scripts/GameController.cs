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
    /// Event called by PlayerController script whenever a ship is created.
    /// </summary>
    /// <param name="playerID">ID of the player that own's the ship.</param>
    public void ShipCreated(int playerID)
    {
        if(OnShipCreated != null)
        {
            OnShipCreated(playerID);
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
