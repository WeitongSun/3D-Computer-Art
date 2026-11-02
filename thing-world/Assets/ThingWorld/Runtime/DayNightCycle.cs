using System;
using UnityEngine;

/// <summary>
/// The one sun of Thing World. Put this on the scene's Directional Light.
/// It turns the light around, dims it at night, and tells every Thing about sunrise and sunset.
///
/// Optional: drag a World Lighting asset (made by the Lighting coordinator) into "Lighting"
/// to control sun colors, ambient colors and fog. The Thing World sky shader follows this
/// cycle automatically.
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(Light))]
public class DayNightCycle : MonoBehaviour
{
    [Tooltip("How many real seconds one full day (24 hours) takes.")]
    public float secondsPerDay = 240f;

    [Range(0f, 24f)]
    [Tooltip("The hour the world starts at.")]
    public float startHour = 9f;

    [Range(0f, 24f)] public float sunriseHour = 6f;
    [Range(0f, 24f)] public float sunsetHour = 18f;

    [Header("Simple settings (used when no World Lighting asset is set)")]
    [Tooltip("Sun brightness during the day.")]
    public float dayIntensity = 1.2f;

    [Tooltip("Sun (moon) brightness at night.")]
    public float nightIntensity = 0.08f;

    [Tooltip("Sky/ambient brightness at night (0–1 of the day value).")]
    [Range(0f, 1f)] public float nightAmbient = 0.25f;

    [Header("World Lighting (optional, made by the Lighting coordinator)")]
    public WorldLighting lighting;

    /// <summary>Fired once when the sun rises.</summary>
    public static event Action Sunrise;

    /// <summary>Fired once when the sun sets.</summary>
    public static event Action Sunset;

    /// <summary>Current hour, 0–24.</summary>
    public static float Hour { get; private set; } = 12f;

    /// <summary>True between sunrise and sunset.</summary>
    public static bool IsDay { get; private set; } = true;

    /// <summary>0 at night, 1 during the day, smooth in between.</summary>
    public static float DayAmount { get; private set; } = 1f;

    static readonly int DayId = Shader.PropertyToID("_TW_Day");
    static readonly int SunDirId = Shader.PropertyToID("_TW_SunDir");
    static readonly int SunColorId = Shader.PropertyToID("_TW_SunColor");

    Light sun;
    float dayAmbient = 1f;

    void Awake()
    {
        sun = GetComponent<Light>();
        if (!Application.isPlaying) return;

        sun.type = LightType.Directional;
        RenderSettings.sun = sun;
        dayAmbient = RenderSettings.ambientIntensity;
        Hour = startHour;
        IsDay = Hour >= sunriseHour && Hour < sunsetHour;
        Apply(true);
    }

    void OnDestroy()
    {
        if (!Application.isPlaying) return;
        // Static events outlive scenes; clear them when the sun is gone.
        if (RenderSettings.sun == sun)
        {
            Sunrise = null;
            Sunset = null;
        }
    }

    void OnValidate()
    {
        // Edit mode: preview the start hour on the sky without moving anything in the scene.
        if (!Application.isPlaying) PreviewInEditor();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            PreviewInEditor();
            return;
        }

        Hour += 24f * Time.deltaTime / Mathf.Max(1f, secondsPerDay);
        if (Hour >= 24f) Hour -= 24f;

        bool dayNow = Hour >= sunriseHour && Hour < sunsetHour;
        if (dayNow != IsDay)
        {
            IsDay = dayNow;
            if (dayNow) Sunrise?.Invoke(); else Sunset?.Invoke();
        }
        Apply(true);
    }

    void PreviewInEditor()
    {
        if (sun == null) sun = GetComponent<Light>();
        Hour = startHour;
        Apply(false);
    }

    void Apply(bool changeScene)
    {
        // 6:00 → sun on the horizon, 12:00 → overhead, 18:00 → horizon again.
        float angle = (Hour - 6f) / 24f * 360f;
        Quaternion rot = Quaternion.Euler(angle, 30f, 0f);

        // Smooth fade over an hour around sunrise and sunset.
        float t = Mathf.Clamp01(Mathf.Min(Hour - sunriseHour + 0.5f, sunsetHour - Hour + 0.5f));
        DayAmount = t;

        // How close the sun is to the horizon (1 = on the horizon), for sunset colors.
        float elevation = Mathf.Sin(angle * Mathf.Deg2Rad);
        float horizonGlow = Mathf.Clamp01(1f - Mathf.Abs(elevation) * 3f);

        Color sunColor = Color.white;
        float intensity = Mathf.Lerp(nightIntensity, dayIntensity, t);
        if (lighting != null)
        {
            sunColor = Color.Lerp(lighting.nightSunColor, lighting.daySunColor, t);
            sunColor = Color.Lerp(sunColor, lighting.sunsetColor, horizonGlow * lighting.sunsetStrength);
            intensity = Mathf.Lerp(lighting.nightIntensity, lighting.dayIntensity, t);
        }

        // Globals for the Thing World sky shader (harmless in edit mode: nothing is saved).
        Shader.SetGlobalFloat(DayId, t);
        Shader.SetGlobalVector(SunDirId, rot * Vector3.back); // direction pointing to the sun
        Shader.SetGlobalColor(SunColorId, sunColor);

        if (!changeScene) return;

        transform.rotation = rot;
        sun.color = sunColor;
        sun.intensity = intensity;

        if (lighting != null)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = Color.Lerp(lighting.nightAmbientSky, lighting.dayAmbientSky, t);
            RenderSettings.ambientEquatorColor = Color.Lerp(lighting.nightAmbientHorizon, lighting.dayAmbientHorizon, t);
            RenderSettings.ambientGroundColor = Color.Lerp(lighting.nightAmbientGround, lighting.dayAmbientGround, t);

            RenderSettings.fog = lighting.fog;
            if (lighting.fog)
            {
                RenderSettings.fogMode = FogMode.ExponentialSquared;
                RenderSettings.fogColor = Color.Lerp(lighting.nightFogColor, lighting.dayFogColor, t);
                RenderSettings.fogDensity = Mathf.Lerp(lighting.nightFogDensity, lighting.dayFogDensity, t);
            }
        }
        else
        {
            RenderSettings.ambientIntensity = dayAmbient * Mathf.Lerp(nightAmbient, 1f, t);
        }
    }
}
