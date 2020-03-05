using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGhostController : MonoBehaviour
{
    Transform[] ghosts = new Transform[8];
    public Renderer[] renderers;
    public Camera cam;
    public Vector3 viewportPosition;
    public Vector3 newPosition;
    public bool isWrappingX = false;
    public bool isWrappingY = false;
    public bool isVisible;

    public float screenWidth;
    public float screenHeight;
    public Vector3 screenBottomLeft;
    public Vector3 screenTopRight;

    void Start()
    {
        cam = Camera.main;
        List<Renderer> tempList = new List<Renderer>();
        foreach (Transform trans in transform)
        {
            tempList.Add(trans.GetComponent<Renderer>());
        }
        renderers = tempList.ToArray();
        viewportPosition = cam.WorldToViewportPoint(transform.position);

        screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        screenWidth = Camera.main.orthographicSize * 2f * Screen.width / Screen.height;
        screenHeight = Camera.main.orthographicSize * 2f;
        CreateGhostShips();
        PositionGhostShips();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "GameArea")
        {
            SwapShips();
        }
    }

    private void Update()
    {
        //ScreenWrap();
        //SwapShips();
        PositionGhostShips();
    }

    bool CheckRenderers()
    {
        foreach (Renderer renderer in renderers)
        {
            // If at least one render is visible, return true
            if (renderer.isVisible)
            {
                return true;
            }
        }

        // Otherwise, the object is invisible
        return false;
    }

    void ScreenWrap()
    {
        isVisible = CheckRenderers();

        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingX && isWrappingY)
        {
            return;
        }

        viewportPosition = cam.WorldToViewportPoint(transform.position);
        newPosition = transform.position;

        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;

            isWrappingX = true;
        }

        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;

            isWrappingY = true;
        }

        transform.position = newPosition;
    }

    void CreateGhostShips()
    {
        for (int i = 0; i < 8; i++)
        {
            ghosts[i] = Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;
            DestroyImmediate(ghosts[i].GetComponent<ShipGhostController>());
            DestroyImmediate(ghosts[i].GetComponent<PlayerTestMovement>());
            ghosts[i].GetComponent<ShipTrailController>().original = transform.GetComponent<PlayerTestMovement>();
            
        }
    }

    void PositionGhostShips()
    {
        // All ghost positions will be relative to the ships (this) transform,
        // so let's star with that.
        Vector3 ghostPosition = transform.position;

        // We're positioning the ghosts clockwise behind the edges of the screen.
        // Let's start with the far right.
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[0].position = ghostPosition;

        // Bottom-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.z = transform.position.z - screenHeight;
        ghosts[1].position = ghostPosition;

        // Bottom
        ghostPosition.x = transform.position.x;
        ghostPosition.z = transform.position.z - screenHeight;
        ghosts[2].position = ghostPosition;

        // Bottom-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.z = transform.position.z - screenHeight;
        ghosts[3].position = ghostPosition;

        // Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.z = transform.position.z;
        ghosts[4].position = ghostPosition;

        // Top-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.z = transform.position.z + screenHeight;
        ghosts[5].position = ghostPosition;

        // Top
        ghostPosition.x = transform.position.x;
        ghostPosition.z = transform.position.z + screenHeight;
        ghosts[6].position = ghostPosition;

        // Top-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.z = transform.position.z + screenHeight;
        ghosts[7].position = ghostPosition;

        // All ghost ships should have the same rotation as the main ship
        for (int i = 0; i < 8; i++)
        {
            ghosts[i].rotation = transform.rotation;
        }
    }

    void SwapShips()
    {
        //isVisible = CheckRenderers();
        //if (isVisible)
        //{
        //    return;
        //}
        bool swapped = false;
        foreach (Transform ghost in ghosts)
        {
            if (ghost.position.x < screenWidth && ghost.position.x > -screenWidth &&
                ghost.position.z < screenHeight && ghost.position.z> -screenHeight)
            {
                transform.position = ghost.position;
                swapped = true;
                break;
            }
        }
        if (swapped)
        {
            Debug.Log("Swapped!");
            PositionGhostShips();
        }
    }
}
