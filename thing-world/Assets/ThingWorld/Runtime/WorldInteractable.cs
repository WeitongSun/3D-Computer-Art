using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// THING WORLD — an object in the world that reacts. No code needed: everything is set in the Inspector.
///
/// WHEN (tick the ones you want):            DO (fill in the reactions you want):
///   · the visitor comes near / leaves          · show, hide or toggle an object
///   · the visitor points at it + grip button    · play a sound
///   · the visitor picks it up / lets go         · play particles
///   · a Thing comes near                        · open/close a Simple Mover (doors, lifts)
///                                               · fire an Animator trigger
///                                               · anything else, with "More"
///
/// Add it with "Thing World → 6. Make Selected Object Interactive".
/// </summary>
[DisallowMultipleComponent]
public class WorldInteractable : MonoBehaviour
{
    [Serializable]
    public class Reaction
    {
        [Tooltip("Turn this object ON.")] public GameObject show;
        [Tooltip("Turn this object OFF.")] public GameObject hide;
        [Tooltip("Switch this object on/off.")] public GameObject toggle;
        [Tooltip("Play this sound from the object.")] public AudioClip sound;
        [Tooltip("Play these particles.")] public ParticleSystem particles;
        [Tooltip("Open/close this Simple Mover (door, lift, drawer…).")] public SimpleMover mover;
        [Tooltip("Animator to send a trigger to.")] public Animator animator;
        [Tooltip("Name of the Animator trigger.")] public string animatorTrigger = "";
        [Tooltip("Anything else: click + and pick an object and a function.")] public UnityEvent more = new UnityEvent();
    }

    [Header("WHEN the visitor comes near")]
    public bool reactToVisitorNear = true;
    [Tooltip("Meters.")] public float nearDistance = 2f;
    public Reaction visitorComesNear = new Reaction();
    public Reaction visitorLeaves = new Reaction();

    [Header("WHEN the visitor points at it and presses the grip (side) button")]
    public bool canBeTouched = false;
    public Reaction touched = new Reaction();

    [Header("WHEN the visitor picks it up")]
    [Tooltip("The object becomes a physical thing you can grab and throw.")]
    public bool canBePickedUp = false;
    public Reaction pickedUp = new Reaction();
    public Reaction letGo = new Reaction();

    [Header("WHEN a Thing comes near")]
    public bool reactToThings = false;
    [Tooltip("Meters.")] public float thingDistance = 2f;
    public Reaction thingComesNear = new Reaction();

    [Header("Timing")]
    [Tooltip("Seconds before the same reaction can happen again.")]
    public float cooldown = 1f;
    [Tooltip("Each reaction happens only the first time.")]
    public bool onlyOnce = false;

    const float SenseInterval = 0.2f;
    const float KillY = -30f;

    AudioSource audioSource;
    Rigidbody body;
    bool visitorNear;
    readonly HashSet<Thing> thingsNear = new HashSet<Thing>();
    readonly Dictionary<Reaction, float> lastRun = new Dictionary<Reaction, float>();
    readonly HashSet<Reaction> done = new HashSet<Reaction>();
    float senseTimer;
    Vector3 startPosition;
    Quaternion startRotation;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        if (canBePickedUp) SetUpPickup();
        else if (canBeTouched) SetUpTouch();
    }

    void SetUpPickup()
    {
        // A grabbable object needs a convex collider and a physical body.
        Collider[] cols = GetComponentsInChildren<Collider>();
        if (cols.Length == 0) gameObject.AddComponent<BoxCollider>();
        foreach (MeshCollider mc in GetComponentsInChildren<MeshCollider>()) mc.convex = true;

        body = GetComponent<Rigidbody>();
        if (body == null) body = gameObject.AddComponent<Rigidbody>();
        body.isKinematic = false;
        body.useGravity = true;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        var grab = GetComponent<XRGrabInteractable>();
        if (grab == null) grab = gameObject.AddComponent<XRGrabInteractable>();
        grab.movementType = XRBaseInteractable.MovementType.Instantaneous;
        grab.throwOnDetach = true;
        grab.useDynamicAttach = true;
        grab.selectEntered.AddListener(_ => Run(pickedUp));
        grab.selectExited.AddListener(_ => { if (!grab.isSelected) Run(letGo); });
    }

    void SetUpTouch()
    {
        if (GetComponentsInChildren<Collider>().Length == 0) gameObject.AddComponent<BoxCollider>();
        var simple = GetComponent<XRSimpleInteractable>();
        if (simple == null) simple = gameObject.AddComponent<XRSimpleInteractable>();
        simple.selectEntered.AddListener(_ => Run(touched));
    }

    void Update()
    {
        senseTimer -= Time.deltaTime;
        if (senseTimer > 0f) return;
        senseTimer = SenseInterval;

        if (reactToVisitorNear && ThingWorldPlayer.Exists)
        {
            Vector3 d = ThingWorldPlayer.HeadPosition - transform.position;
            float flat = new Vector2(d.x, d.z).magnitude;
            bool closeNow = flat < nearDistance && Mathf.Abs(d.y) < nearDistance + 2f;
            bool farNow = flat > nearDistance * 1.25f || Mathf.Abs(d.y) > nearDistance + 3f;
            if (!visitorNear && closeNow) { visitorNear = true; Run(visitorComesNear); }
            else if (visitorNear && farNow) { visitorNear = false; Run(visitorLeaves); }
        }

        if (reactToThings)
        {
            float r2 = thingDistance * thingDistance;
            foreach (Thing t in Thing.All.ToArray())
            {
                if (t == null) continue;
                bool close = (t.transform.position - transform.position).sqrMagnitude < r2;
                if (close && thingsNear.Add(t)) Run(thingComesNear);
                else if (!close) thingsNear.Remove(t);
            }
        }

        // A thrown object that falls out of the world comes back.
        if (body != null && transform.position.y < KillY)
        {
            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            transform.SetPositionAndRotation(startPosition, startRotation);
        }
    }

    /// <summary>Run a reaction (also callable from another object's "More" list).</summary>
    public void Run(Reaction r)
    {
        if (r == null) return;
        if (onlyOnce && done.Contains(r)) return;
        if (lastRun.TryGetValue(r, out float last) && Time.time - last < cooldown) return;
        lastRun[r] = Time.time;
        done.Add(r);

        try
        {
            if (r.show != null) r.show.SetActive(true);
            if (r.hide != null) r.hide.SetActive(false);
            if (r.toggle != null) r.toggle.SetActive(!r.toggle.activeSelf);
            if (r.sound != null) PlaySound(r.sound);
            if (r.particles != null) r.particles.Play();
            if (r.mover != null) r.mover.Toggle();
            if (r.animator != null && !string.IsNullOrEmpty(r.animatorTrigger)) r.animator.SetTrigger(r.animatorTrigger);
            r.more?.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"[{name}] Interactable reaction failed: {e.Message}", this);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 20f;
        }
        audioSource.PlayOneShot(clip);
    }

    void OnDrawGizmosSelected()
    {
        if (reactToVisitorNear)
        {
            Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.6f);
            Gizmos.DrawWireSphere(transform.position, nearDistance);
        }
        if (reactToThings)
        {
            Gizmos.color = new Color(1f, 0.6f, 0.1f, 0.6f);
            Gizmos.DrawWireSphere(transform.position, thingDistance);
        }
    }
}
