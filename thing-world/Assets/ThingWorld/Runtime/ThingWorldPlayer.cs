using UnityEngine;

/// <summary>
/// Where is the person wearing the headset? Things ask this class.
/// It uses the camera tagged "MainCamera" (the XR Origin's camera).
/// </summary>
public static class ThingWorldPlayer
{
    static Camera cachedCamera;

    static Transform Head
    {
        get
        {
            if (cachedCamera == null || !cachedCamera.isActiveAndEnabled) cachedCamera = Camera.main;
            return cachedCamera != null ? cachedCamera.transform : null;
        }
    }

    /// <summary>True if there is a player camera in the scene.</summary>
    public static bool Exists => Head != null;

    /// <summary>Position of the player's eyes.</summary>
    public static Vector3 HeadPosition => Head != null ? Head.position : Vector3.zero;

    /// <summary>Position on the ground under the player's head.</summary>
    public static Vector3 FeetPosition
    {
        get
        {
            Transform h = Head;
            if (h == null) return Vector3.zero;
            Vector3 p = h.position;
            if (Physics.Raycast(p, Vector3.down, out RaycastHit hit, 5f, ~0, QueryTriggerInteraction.Ignore)) return hit.point;
            return new Vector3(p.x, p.y - 1.6f, p.z);
        }
    }

    /// <summary>Horizontal distance from a point to the player, in meters.</summary>
    public static float DistanceTo(Vector3 point)
    {
        Transform h = Head;
        if (h == null) return float.MaxValue;
        Vector3 d = h.position - point;
        d.y = 0f;
        return d.magnitude;
    }
}
