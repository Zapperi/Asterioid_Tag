using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestMovement : MonoBehaviour
{
    public GameObject player;
    public Rigidbody playerRB;
    public bool original = true;

    public bool dampeners = false;
    public float rotateSpeed;
    public float maxSpeed;
    public float acceleration = 10f;
    public float drag = 0f;
    public float currentSpeed;
    public float newSpeed;

    public Vector3 direction;
    public Vector3 sumVector;
    [Range(0, 1)]
    public float directionMultiplier;
    public float accelerationMultiplier;

    //public TrailRenderer[] trailRenderers;
    //public Transform[] trailParents;
    // Start is called before the first frame update
    void Start()
    {
        //trailParents = new Transform[trailRenderers.Length];
        //for (int i = 0; i < trailParents.Length; ++i)
        //{
        //    trailParents[i] = trailRenderers[i].transform.parent.transform;
        //}
    }
    private void Update()
    {
        if (Input.GetButton("Gas"))
        {
            dampeners = !dampeners;
        }
    }

    void FixedUpdate()
    {
        currentSpeed = playerRB.velocity.magnitude;
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");
        player.transform.Rotate(0f, moveHorizontal * rotateSpeed * Time.fixedDeltaTime * 100f, 0f);
        //playerRB.AddTorque(0f, moveHorizontal * rotateSpeed * Time.fixedDeltaTime * 100f, 0f);        

        float moveVertical = 0f;
        if (Input.GetAxis("GasAxis") != 0)
        {
            //Store the current vertical input in the float moveVertical.
            moveVertical = Input.GetAxis("GasAxis");
        }

        if (dampeners && Input.GetAxis("GasAxis") == 0f || playerRB.velocity.magnitude > maxSpeed)
        {
            playerRB.drag = 0.5f;
        }
        else if (Input.GetAxis("GasAxis") != 0f)
        {
            playerRB.drag = 0f;
            //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
            playerRB.AddForce(player.transform.forward * moveVertical * acceleration * Time.deltaTime * 100f, ForceMode.Acceleration);
        }

        //for (int i = 0; i < trailParents.Length; ++i)
        //{
        //    if (!IsVisibleToCamera(trailParents[i]))
        //    {
        //        trailRenderers[i].emitting = false;
        //    }
        //    else
        //    {
        //        trailRenderers[i].emitting = true;
        //        trailRenderers[i].time = currentSpeed / maxSpeed;
        //    }
        //}
    }

    //public static bool IsVisibleToCamera(Transform transform)
    //{
    //    Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
    //    return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    //}
}
