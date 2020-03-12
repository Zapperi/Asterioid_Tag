using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Renderer cockpitRenderer;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {        
        cockpitRenderer.material.SetColor("_BaseColor", GameController.instance.players[player.playerID-1].playerColor);
    }
    
    /// <summary>
    /// Sets the owner of the ship.
    /// </summary>
    /// <param name="owner">Player that owns the ship.</param>
    public void InitShip(PlayerController owner)
    {
        player = owner;
    }
}
