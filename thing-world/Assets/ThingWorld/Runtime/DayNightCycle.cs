using System;
using UnityEngine;

/// <summary>
/// The one sun of Thing World. Put this on the scene's Directional Light.
/// It turns the light around, dims it at night, and tells every Thing about sunrise and sunset.
/// </summary>
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

    [Tooltip("Sun brightness during the day.")]
    public float dayIntensity = 1.2f;

    [Tooltip("Sun (moon) brightness at night.")]
    public float nightIntensity = 0.08f;

    [Tooltip("Sky/ambient brightness at night (0–1 of the day value).")]
    [Range(0f, 1f)] public float nightAmbient = 0.25f;

    /// <summary>Fired once when the sun rises.</summary>
    public static event Action Sunrise;

    /// <summary>Fired once when the sun sets.</summary>
    public static event Action Sunset;

    /// <summary>Current hour, 0–24.</summary>
    public static float Hour { get; private set; } = 12f;

    /// <summary>True between sunrise and sunset.</summary>
    public static bool IsDay { get; private set; } = true;

    Light sun;
    float dayAmbient = 1f;

    void Awake()
    {
        sun = GetComponent<Light>();
        sun.type = LightType.Directional;
        RenderSettings.sun = sun;
        dayAmbient = RenderSettings.ambientIntensity;
        Hour = startHour;
        IsDay = Hour >= sunriseHour && Hour < sunsetHour;
        Apply();
    }

    void OnDestroy()
    {
        // Static events outlive scenes; clear them when the sun is gone.
        if (RenderSettings.sun == sun)
        {
            Sunrise = null;
            Sunset = null;
        }
    }

    void Update()
    {
        Hour += 24f * Time.deltaTime / Mathf.Max(1f, secondsPerDay);
        if (Hour >= 24f) Hour -= 24f;

        bool dayNow = Hour >= sunriseHour && Hour < sunsetHour;
        if (dayNow != IsDay)
        {
            IsDay = dayNow;
            if (dayNow) Sunrise?.Invoke(); else Sunset?.Invoke();
        }
        Apply();
    }

    void Apply()
    {
        // 6:00 → sun on the horizon, 12:00 → overhead, 18:00 → horizon again.
        float angle = (Hour - 6f) / 24f * 360f;
        transform.rotation = Quaternion.Euler(angle, 30f, 0f);

        // Smooth fade over an hour around sunrise and sunset.
        float t = Mathf.Clamp01(Mathf.Min(Hour - sunriseHour + 0.5f, sunsetHour - Hour + 0.5f));
        sun.intensity = Mathf.Lerp(nightIntensity, dayIntensity, t);
        RenderSettings.ambientIntensity = dayAmbient * Mathf.Lerp(nightAmbient, 1f, t);
    }
}
