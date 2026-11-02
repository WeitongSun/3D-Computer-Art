using UnityEngine;

/// <summary>
/// THING WORLD — makes a particle effect follow the day/night cycle.
/// Put it on the same object as the Particle System (the Particles tool adds it for you).
/// Example: fireflies only at night, dust only in the day, a glow at sunrise and sunset.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(ParticleSystem))]
public class DayNightParticles : MonoBehaviour
{
    public enum When { Always, OnlyInTheDay, OnlyAtNight, AtSunriseAndSunset }

    [Tooltip("When should this effect be visible?")]
    public When showWhen = When.Always;

    [Tooltip("Seconds to fade in or out.")]
    public float fadeSeconds = 3f;

    ParticleSystem ps;
    float fullRate;
    float amount;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        fullRate = ps.emission.rateOverTime.constant;
        amount = Target();
        Apply();
    }

    void Update()
    {
        amount = Mathf.MoveTowards(amount, Target(), Time.deltaTime / Mathf.Max(0.1f, fadeSeconds));
        Apply();
    }

    float Target()
    {
        float day = DayNightCycle.DayAmount; // 0 night … 1 day
        switch (showWhen)
        {
            case When.OnlyInTheDay: return day;
            case When.OnlyAtNight: return 1f - day;
            case When.AtSunriseAndSunset: return 1f - Mathf.Abs(day * 2f - 1f); // peaks while day ↔ night changes
            default: return 1f;
        }
    }

    void Apply()
    {
        var emission = ps.emission;
        emission.rateOverTime = fullRate * amount;
    }
}
