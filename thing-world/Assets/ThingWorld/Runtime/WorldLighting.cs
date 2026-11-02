using UnityEngine;

/// <summary>
/// The look of light in Thing World, made by the Lighting coordinator.
/// Create one with "Thing World → Coordinators → Lighting: Create Lighting Settings",
/// then drag it into the "Lighting" slot of the Sun (Day Night Cycle) in a scene.
/// Change values while playing to see them live (changes made in Play mode on an asset ARE kept).
/// </summary>
[CreateAssetMenu(menuName = "Thing World/World Lighting", fileName = "WorldLighting")]
public class WorldLighting : ScriptableObject
{
    [Header("Sun")]
    public Color daySunColor = new Color(1f, 0.96f, 0.88f);
    [Range(0f, 3f)] public float dayIntensity = 1.2f;
    [Tooltip("The moon: the sun light's color at night.")]
    public Color nightSunColor = new Color(0.55f, 0.65f, 1f);
    [Range(0f, 1f)] public float nightIntensity = 0.12f;

    [Header("Sunrise / sunset")]
    public Color sunsetColor = new Color(1f, 0.55f, 0.3f);
    [Range(0f, 1f)] public float sunsetStrength = 0.7f;

    [Header("Ambient light (fills the shadows)")]
    public Color dayAmbientSky = new Color(0.62f, 0.72f, 0.86f);
    public Color dayAmbientHorizon = new Color(0.55f, 0.55f, 0.5f);
    public Color dayAmbientGround = new Color(0.3f, 0.27f, 0.22f);
    public Color nightAmbientSky = new Color(0.1f, 0.12f, 0.22f);
    public Color nightAmbientHorizon = new Color(0.08f, 0.08f, 0.14f);
    public Color nightAmbientGround = new Color(0.04f, 0.04f, 0.06f);

    [Header("Fog (keep it light: dense fog hides the world in VR)")]
    public bool fog = false;
    public Color dayFogColor = new Color(0.75f, 0.8f, 0.85f);
    public Color nightFogColor = new Color(0.08f, 0.09f, 0.14f);
    [Range(0f, 0.05f)] public float dayFogDensity = 0.01f;
    [Range(0f, 0.05f)] public float nightFogDensity = 0.02f;
}
