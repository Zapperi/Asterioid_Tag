using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScreenWrap : MonoBehaviour
{
    public Renderer[] m_renderers;
    public TrailRenderer[] trailRenderers;

    private bool m_isWrappingX = false;
    private bool m_isWrappingZ = false;
    private bool m_isWrapping = false;
    public bool m_objectHasTrails;
    private Camera m_cam;

    private float screenWidth;
    private float screenHeight;
    private float originalTrailTime;

    void Start()
    {
        m_cam = Camera.main;
        // Fetch all the renderers that display ship graphics.
        // In the demo we only have one mesh for the ship and thus
        // only one renderer.
        // We could have a complicated ship, made out of several meshes
        // and this would fetch all the renderers.
        // We use the renderer(s) so we can check if the ship is
        // visible or not.
        List<Renderer> tempList = new List<Renderer>();
        foreach (Transform trans in transform)
        {
            tempList.Add(trans.GetComponent<Renderer>());
        }
        m_renderers = tempList.ToArray();
        trailRenderers = GetComponentsInChildren<TrailRenderer>();
        if (trailRenderers.Length > 0)
        {
            m_objectHasTrails = true;
        }
        screenWidth = GameController.instance.screenWidth;
        screenHeight = GameController.instance.screenHeight;
        originalTrailTime = trailRenderers[0].time;
    }

    // Update is called once per frame
    void Update()
    {        
        ScreenWrap();
    }

    void ScreenWrap()
    {
        // If all parts of the object are invisible we wrap it around
        foreach (Renderer renderer in m_renderers)
        {
            // If at least one render is invisible, return false
            if (renderer.IsVisibleFrom(m_cam))
            {
                m_isWrappingX = false;
                m_isWrappingZ = false;
                return;
            }
        }
        if (!m_isWrapping)
        {
            StartCoroutine(WrapObject());
        }
    }


    IEnumerator WrapObject()
    {
        m_isWrapping = true;

        ReplaceTrailRenderers();

        yield return null;
        Vector3 newPosition = transform.position;
        Vector3 viewportPosition = m_cam.WorldToViewportPoint(transform.position);

        // Wrap is off screen along the x-axis and is not being wrapped already
        if (!m_isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;
            m_isWrappingX = true;
        }

        // Wrap is off screen along the z-axis and is not being wrapped already
        else if (!m_isWrappingZ && (viewportPosition.z > 1 || viewportPosition.z < 0))
        {
            newPosition.z = -newPosition.z;
            m_isWrappingZ = true;
        }
        //Apply new position        
        transform.position = newPosition;
        yield return null;

        ActivateTrailRenderers();

        yield return new WaitForSeconds(0.1f);
        m_isWrapping = false;
    }

    private void ReplaceTrailRenderers()
    {
        if (m_objectHasTrails)
        {
            for (int i = 0; i < trailRenderers.Length; ++i)
            {
                GameObject newTrailGO;
                newTrailGO = Instantiate(trailRenderers[i].gameObject, trailRenderers[i].transform.position, trailRenderers[i].transform.rotation, trailRenderers[i].transform.parent) as GameObject;
                TrailRenderer newTrail = newTrailGO.GetComponent<TrailRenderer>();
                //newTrail.widthCurve = trailRenderers[i].widthCurve;
                //newTrail.time = trailRenderers[i].time;
                //newTrail.minVertexDistance = trailRenderers[i].minVertexDistance;

                //newTrail.colorGradient = trailRenderers[i].colorGradient;
                //newTrail.numCornerVertices = trailRenderers[i].numCornerVertices;
                //newTrail.materials = trailRenderers[i].materials;
                newTrail.emitting = false;                
                //newTrail.time = 0;
                trailRenderers[i].transform.parent = null;
                trailRenderers[i].autodestruct = true;                
                trailRenderers[i] = newTrail;
            }
        }
    }

    private void ActivateTrailRenderers()
    {
        if (m_objectHasTrails)
        {
            foreach (TrailRenderer trail in trailRenderers)
            {                
                trail.emitting = true;
            }
        }
    }
}
