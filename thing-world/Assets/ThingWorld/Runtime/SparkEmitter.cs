using UnityEngine;

/// <summary>
/// The particle burst used by Spark(). Created automatically the first time a Thing sparks.
/// </summary>
public class SparkEmitter : MonoBehaviour
{
    ParticleSystem particles;

    internal static SparkEmitter Create(Thing owner)
    {
        var go = new GameObject("Sparks");
        go.transform.SetParent(owner.transform, false);
        var emitter = go.AddComponent<SparkEmitter>();

        var ps = go.AddComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.playOnAwake = false;
        main.loop = false;
        main.duration = 1f;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.6f, 1.2f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(1.5f, 4f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.04f, 0.12f);
        main.gravityModifier = 0.4f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 400;

        var emission = ps.emission;
        emission.rateOverTime = 0f;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.2f;

        var size = ps.sizeOverLifetime;
        size.enabled = true;
        size.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));

        var renderer = go.GetComponent<ParticleSystemRenderer>();
        ThingWorldConfig config = ThingWorldConfig.Load();
        if (config != null && config.sparkMaterial != null) renderer.sharedMaterial = config.sparkMaterial;
        else Debug.LogWarning("[Thing World] No spark material. Run 'Thing World → Instructor → Set Up Project'.");

        emitter.particles = ps;
        return emitter;
    }

    internal void Burst(Color color, int amount, float height)
    {
        transform.localPosition = Vector3.zero;
        transform.position = transform.parent.position + Vector3.up * height;
        var p = new ParticleSystem.EmitParams { startColor = color, applyShapeToPosition = true };
        particles.Emit(p, amount);
    }
}
