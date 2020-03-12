using UnityEngine;

public static class RendererExtensions
{
    /// <summary>
    /// Checks if renderer is visible from the target Camera.
    /// </summary>
    /// <param name="renderer">Renderer to check.</param>
    /// <param name="camera">Camera to check.</param>
    /// <returns>True if renderer is seen by the camera.</returns>
    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    public static float NominalScreenWidthAt(this Camera c, Transform t)
    {
        float yFromCamera = t.transform.position.z - c.transform.position.z;

        return
            c.ViewportToWorldPoint(new Vector3(1f, 1f, yFromCamera)).x
            - c.ViewportToWorldPoint(new Vector3(0f, 1f, yFromCamera)).x;
    }

    public static float NominalScreenHeightAt(this Camera c, Transform t)
    {
        float yFromCamera = t.transform.position.z - c.transform.position.z;

        return
            c.ViewportToWorldPoint(new Vector3(0f, 1f, yFromCamera)).y
            - c.ViewportToWorldPoint(new Vector3(0f, 0f, yFromCamera)).y;
    }
}
