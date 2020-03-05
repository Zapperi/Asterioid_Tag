using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTrailController : MonoBehaviour
{
    public TrailRenderer[] trailRenderers;
    public Transform[] trailParents;
    public PlayerTestMovement original;
    public static float bufferSize = 0f;
    // Start is called before the first frame update
    void Start()
    {
        trailRenderers = GetComponentsInChildren<TrailRenderer>();
        trailParents = new Transform[trailRenderers.Length];
        for (int i = 0; i < trailParents.Length; ++i)
        {
            trailParents[i] = trailRenderers[i].transform.parent.transform;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < trailParents.Length; ++i)
        {
            if (!IsVisibleToCamera(trailParents[i]))
            {
                trailRenderers[i].emitting = false;
            }
            else
            {
                trailRenderers[i].emitting = true;
                trailRenderers[i].time = original.currentSpeed / original.maxSpeed;
            }
        }
    }

    public static bool IsVisibleToCamera(Transform transform)
    {
        Vector3 visTest = Camera.main.WorldToViewportPoint(transform.position);
        return (visTest.x + bufferSize >= 0 && visTest.y  + bufferSize >= 0) && (visTest.x - bufferSize <= 1 && visTest.y - bufferSize <= 1) && visTest.z + bufferSize >= 0;
    }
}
