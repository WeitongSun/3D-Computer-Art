using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// THING WORLD — base class for every creature in the world.
///
/// Students: you never edit this file. Your own script says "public class YourNameThing : Thing"
/// and fills in the events below (OnBorn, OnMeetSomeone, OnGrabbed, ...).
/// Everything difficult — walking, physics, being grabbed, falling, standing up again,
/// sensing neighbors, speech bubbles, sparks, sound, animation — is handled here.
///
/// Inspired by ThingThingThing by ZZYW (https://github.com/ZZYW/ThingThingThing, MIT License).
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(AudioSource))]
public abstract class Thing : MonoBehaviour
{
    // =====================================================================
    //  STUDENT AREA — things you can use in your own script
    // =====================================================================

    /// <summary>Your personality knobs. Change them in Setup().</summary>
    protected ThingSettings settings = new ThingSettings();

    [Header("Your sounds (drag audio files here, then use PlaySound(0), PlaySound(1), ...)")]
    [SerializeField] protected AudioClip[] mySounds = new AudioClip[0];

    /// <summary>The name of your Thing (the name of your script).</summary>
    public string ThingName => GetType().Name;

    /// <summary>All Things currently near you.</summary>
    protected IReadOnlyList<Thing> Neighbors => neighbors;

    /// <summary>How many Things are near you right now.</summary>
    protected int NeighborCount => neighbors.Count;

    /// <summary>True while the player is holding you.</summary>
    protected bool IsGrabbed => state == BodyState.Held;

    /// <summary>True while you are flying or falling after being thrown or dropped.</summary>
    protected bool IsFalling => state == BodyState.Falling;

    /// <summary>True while you are walking somewhere.</summary>
    protected bool IsMoving => state == BodyState.Walking && agent.enabled && agent.velocity.sqrMagnitude > 0.01f;

    /// <summary>True while the player is closer than settings.playerNearRadius.</summary>
    protected bool IsPlayerNear => playerNear;

    /// <summary>Distance to the player in meters (a big number if there is no player).</summary>
    protected float PlayerDistance => ThingWorldPlayer.DistanceTo(transform.position);

    /// <summary>True during the day (between sunrise and sunset).</summary>
    protected bool IsDay => DayNightCycle.IsDay;

    /// <summary>Time of day from 0 to 24. 13.5 means 1:30 PM.</summary>
    protected float TimeOfDay => DayNightCycle.Hour;

    /// <summary>How tall you are, in meters.</summary>
    protected float Height => bodyHeight;

    // ---------------------------------------------------------------------
    //  LIFE CYCLE — override these in your own script
    // ---------------------------------------------------------------------

    /// <summary>Runs once, before anything else. Change your settings here.</summary>
    protected virtual void Setup() { }

    /// <summary>Runs once, when you appear in the world.</summary>
    protected virtual void OnBorn() { }

    /// <summary>Runs every frame (about 72–90 times per second in the headset). Keep it light!</summary>
    protected virtual void EveryFrame() { }

    // ---------------------------------------------------------------------
    //  EVENTS — things that happen to you. Override the ones you care about.
    // ---------------------------------------------------------------------

    /// <summary>Another Thing came close to you.</summary>
    protected virtual void OnMeetSomeone(Thing other) { }

    /// <summary>A Thing that was close to you walked away.</summary>
    protected virtual void OnLeaveSomeone(Thing other) { }

    /// <summary>A neighbor said something.</summary>
    protected virtual void OnNeighborSpeaks(Thing speaker, string words) { }

    /// <summary>A neighbor made sparks.</summary>
    protected virtual void OnNeighborSparks(Thing who) { }

    /// <summary>You reached the place you were walking to.</summary>
    protected virtual void OnArrived() { }

    /// <summary>The sun came up.</summary>
    protected virtual void OnSunrise() { }

    /// <summary>The sun went down.</summary>
    protected virtual void OnSunset() { }

    /// <summary>The player came close to you.</summary>
    protected virtual void OnPlayerNear() { }

    /// <summary>The player walked away from you.</summary>
    protected virtual void OnPlayerLeave() { }

    /// <summary>The player picked you up.</summary>
    protected virtual void OnGrabbed() { }

    /// <summary>The player let go of you. throwSpeed is in meters per second (0 = gently dropped).</summary>
    protected virtual void OnReleased(float throwSpeed) { }

    /// <summary>You landed and stood up again after being dropped or thrown.</summary>
    protected virtual void OnLanded() { }

    // ---------------------------------------------------------------------
    //  ACTIONS — things you can do. Call them from your events.
    // ---------------------------------------------------------------------

    /// <summary>Walk to a point in the world.</summary>
    protected void WalkTo(Vector3 place)
    {
        follow = FollowMode.None;
        manualStop = false;
        lookTarget = null;
        SetDestinationSafe(place);
    }

    /// <summary>Walk to another Thing.</summary>
    protected void WalkTo(Thing other)
    {
        if (other != null) WalkTo(other.transform.position);
    }

    /// <summary>Walk to a random place nearby (within settings.wanderRadius).</summary>
    protected void Wander()
    {
        if (TryRandomPoint(transform.position, settings.wanderRadius, out Vector3 p)) WalkTo(p);
    }

    /// <summary>Keep walking towards the player until told otherwise.</summary>
    protected void FollowPlayer()
    {
        manualStop = false;
        follow = FollowMode.FollowPlayer;
    }

    /// <summary>Keep running away from the player until told otherwise.</summary>
    protected void RunFromPlayer()
    {
        manualStop = false;
        follow = FollowMode.FleePlayer;
    }

    /// <summary>Stop where you are (until you call StartMoving or WalkTo).</summary>
    protected void StopMoving()
    {
        follow = FollowMode.None;
        manualStop = true;
        if (agent.enabled && agent.isOnNavMesh) agent.ResetPath();
    }

    /// <summary>Stop for a number of seconds, then carry on.</summary>
    protected void StopMoving(float seconds)
    {
        StopMoving();
        After(seconds, StartMoving);
    }

    /// <summary>Allow walking again after StopMoving().</summary>
    protected void StartMoving()
    {
        manualStop = false;
        wanderTimer = 0f;
    }

    /// <summary>Change your walking speed (meters per second).</summary>
    protected void SetSpeed(float metersPerSecond)
    {
        settings.walkSpeed = Mathf.Max(0f, metersPerSecond);
        agent.speed = settings.walkSpeed;
    }

    /// <summary>Turn to face a point.</summary>
    protected void LookAt(Vector3 place)
    {
        lookTarget = place;
    }

    /// <summary>Turn to face the player.</summary>
    protected void LookAtPlayer()
    {
        if (ThingWorldPlayer.Exists) lookTarget = ThingWorldPlayer.FeetPosition;
    }

    /// <summary>Say something in a speech bubble.</summary>
    protected void Speak(string words, float seconds = 3f)
    {
        if (string.IsNullOrEmpty(words)) return;
        if (Time.time < nextSpeakTime) return; // small cooldown so Things can't flood the world
        nextSpeakTime = Time.time + 1.5f;

        if (bubble == null) bubble = SpeechBubble.Create(this);
        bubble.Show(words, seconds);

        foreach (Thing t in All.ToArray()) // a copy: listeners may add or remove Things
        {
            if (t != this && t.neighbors.Contains(this)) t.SafeCall(() => t.OnNeighborSpeaks(this, words));
        }
    }

    /// <summary>Burst of glowing particles in your signature color.</summary>
    protected void Spark() => Spark(settings.myColor, 30);

    /// <summary>Burst of glowing particles.</summary>
    protected void Spark(Color color, int amount = 30)
    {
        if (Time.time < nextSparkTime) return;
        nextSparkTime = Time.time + 0.2f;

        if (sparks == null) sparks = SparkEmitter.Create(this);
        sparks.Burst(color, Mathf.Clamp(amount, 1, 200), bodyHeight * 0.6f);

        foreach (Thing t in All.ToArray()) // a copy: listeners may add or remove Things
        {
            if (t != this && t.neighbors.Contains(this)) t.SafeCall(() => t.OnNeighborSparks(this));
        }
    }

    /// <summary>Play one of your sounds (0 = first sound in the 'My Sounds' list).</summary>
    protected void PlaySound(int index)
    {
        if (mySounds == null || mySounds.Length == 0)
        {
            Warn("PlaySound: drag some audio files into 'My Sounds' on your prefab first.");
            return;
        }
        PlaySound(mySounds[Mathf.Abs(index) % mySounds.Length]);
    }

    /// <summary>Play a random one of your sounds.</summary>
    protected void PlayRandomSound()
    {
        if (mySounds != null && mySounds.Length > 0) PlaySound(UnityEngine.Random.Range(0, mySounds.Length));
    }

    /// <summary>Play an audio clip from your body.</summary>
    protected void PlaySound(AudioClip clip)
    {
        if (clip == null || Time.time < nextSoundTime) return;
        nextSoundTime = Time.time + 0.3f;
        audioSource.PlayOneShot(clip);
    }

    /// <summary>Play one of your extra animations: 1, 2 or 3 (set them in your Animation Override Controller).</summary>
    protected void PlayAnimation(int actionNumber)
    {
        if (animator == null) return;
        int n = Mathf.Clamp(actionNumber, 1, 3);
        SetAnimatorTrigger("Action" + n);
    }

    /// <summary>Tint your whole body a color.</summary>
    protected void SetColor(Color color)
    {
        if (block == null) block = new MaterialPropertyBlock();
        foreach (Renderer r in bodyRenderers)
        {
            if (r == null || r is ParticleSystemRenderer) continue;
            r.GetPropertyBlock(block);
            block.SetColor(BaseColorId, color);
            block.SetColor(ColorId, color);
            r.SetPropertyBlock(block);
        }
    }

    /// <summary>Go back to your original colors.</summary>
    protected void ResetColor()
    {
        foreach (Renderer r in bodyRenderers)
        {
            if (r == null || r is ParticleSystemRenderer) continue;
            r.SetPropertyBlock(null);
        }
    }

    /// <summary>Grow or shrink. 1 = original size, 2 = twice as big, 0.5 = half.</summary>
    protected void SetSize(float multiplier)
    {
        multiplier = Mathf.Clamp(multiplier, 0.1f, 5f);
        transform.localScale = originalScale * multiplier;
        bodyHeight = baseHeight * multiplier;
    }

    /// <summary>Do something once, after some seconds.  Example: After(2, Spark);</summary>
    protected void After(float seconds, Action action)
    {
        StartCoroutine(AfterRoutine(seconds, action));
    }

    /// <summary>Do something again and again, every few seconds.  Example: Repeat(5, Wander);</summary>
    protected void Repeat(float everySeconds, Action action)
    {
        StartCoroutine(RepeatRoutine(Mathf.Max(0.1f, everySeconds), action));
    }

    /// <summary>Go back to where you were born.</summary>
    protected void GoHome()
    {
        WalkTo(homePosition);
    }

    // =====================================================================
    //  ENGINE ROOM — students don't need to read below this line
    // =====================================================================

    /// <summary>Every Thing that currently exists.</summary>
    public static readonly List<Thing> All = new List<Thing>();

    enum BodyState { Walking, Held, Falling }
    enum FollowMode { None, FollowPlayer, FleePlayer }

    static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
    static readonly int ColorId = Shader.PropertyToID("_Color");
    const float SenseInterval = 0.2f;
    const float KillY = -30f;

    NavMeshAgent agent;
    Rigidbody body;
    CapsuleCollider capsule;
    XRGrabInteractable grab;
    AudioSource audioSource;
    Animator animator;
    Renderer[] bodyRenderers = new Renderer[0];
    MaterialPropertyBlock block;
    SpeechBubble bubble;
    SparkEmitter sparks;
    readonly HashSet<string> animatorParams = new HashSet<string>();

    readonly List<Thing> neighbors = new List<Thing>();
    BodyState state = BodyState.Walking;
    FollowMode follow = FollowMode.None;
    bool manualStop;
    bool playerNear;
    bool hadPath;
    bool started;
    Vector3? lookTarget;
    Vector3 homePosition;
    Vector3 originalScale;
    float baseHeight = 1.7f;
    float bodyHeight = 1.7f;
    float senseTimer;
    float wanderTimer;
    float nextSpeakTime, nextSparkTime, nextSoundTime;
    float fallStartTime, restingTime;
    Vector3 lastHeldPosition, heldVelocity;

    /// <summary>Height of the top of the head, used by speech bubbles.</summary>
    internal float BubbleHeight => bodyHeight + settings.speechBubbleOffset;

    void Awake()
    {
        CheckForHiddenUnityMethods();

        agent = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        grab = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        bodyRenderers = GetComponentsInChildren<Renderer>();
        originalScale = transform.localScale;

        // Things are never part of the walkable ground.
        var modifier = GetComponent<NavMeshModifier>();
        if (modifier == null) modifier = gameObject.AddComponent<NavMeshModifier>();
        modifier.ignoreFromBuild = true;

        // Physics: the agent moves us; physics only takes over when we are thrown.
        body.isKinematic = true;
        body.useGravity = true;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        body.mass = 5f;

        agent.enabled = false; // turned on in Start, once we are standing on the NavMesh
        agent.autoBraking = true;
        agent.stoppingDistance = 0.3f;

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 20f;

        grab.movementType = XRBaseInteractable.MovementType.Instantaneous;
        grab.throwOnDetach = true;
        grab.useDynamicAttach = true;
        grab.selectEntered.AddListener(HandleGrabbed);
        grab.selectExited.AddListener(HandleReleased);

        if (animator != null)
        {
            animator.applyRootMotion = false;
            foreach (AnimatorControllerParameter p in animator.parameters) animatorParams.Add(p.name);
        }

        MeasureHeight();
        SafeCall(Setup);
    }

    void OnEnable()
    {
        if (!All.Contains(this)) All.Add(this);
        DayNightCycle.Sunrise += HandleSunrise;
        DayNightCycle.Sunset += HandleSunset;
    }

    void OnDisable()
    {
        All.Remove(this);
        DayNightCycle.Sunrise -= HandleSunrise;
        DayNightCycle.Sunset -= HandleSunset;
        foreach (Thing t in All) t.neighbors.Remove(this);
        neighbors.Clear();
    }

    void OnDestroy()
    {
        if (grab != null)
        {
            grab.selectEntered.RemoveListener(HandleGrabbed);
            grab.selectExited.RemoveListener(HandleReleased);
        }
    }

    void Start()
    {
        agent.speed = settings.walkSpeed;
        agent.angularSpeed = settings.turnSpeed;
        agent.acceleration = Mathf.Max(4f, settings.walkSpeed * 4f);
        // Agent sizes are in local units (the agent scales with the object), like the capsule.
        agent.radius = Mathf.Max(0.15f, capsule.radius);
        agent.height = Mathf.Max(0.3f, capsule.height);
        grab.enabled = settings.canBeGrabbed;

        PlaceOnNavMesh(transform.position);
        homePosition = transform.position;
        wanderTimer = UnityEngine.Random.Range(0f, settings.wanderInterval); // so Things don't all move at once
        senseTimer = UnityEngine.Random.Range(0f, SenseInterval);
        started = true;
        SafeCall(OnBorn);
    }

    void Update()
    {
        if (!started) return;

        switch (state)
        {
            case BodyState.Walking: UpdateWalking(); break;
            case BodyState.Held: UpdateHeld(); break;
        }

        senseTimer -= Time.deltaTime;
        if (senseTimer <= 0f)
        {
            senseTimer = SenseInterval;
            SenseNeighbors();
            SensePlayer();
            UpdateFollow();
        }

        UpdateAnimator();
        SafeCall(EveryFrame);
    }

    void FixedUpdate()
    {
        if (state == BodyState.Falling) UpdateFalling();
    }

    // ---------------- walking ----------------

    void UpdateWalking()
    {
        if (!agent.enabled || !agent.isOnNavMesh) return;
        agent.speed = settings.walkSpeed;

        // Wandering on our own (following/fleeing the player is handled in UpdateFollow)
        if (settings.autoWander && !manualStop && follow == FollowMode.None)
        {
            wanderTimer -= Time.deltaTime;
            bool idle = !agent.pathPending && (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance);
            if (wanderTimer <= 0f && idle)
            {
                wanderTimer = settings.wanderInterval;
                if (TryRandomPoint(transform.position, settings.wanderRadius, out Vector3 p)) SetDestinationSafe(p);
            }
        }

        // Arrived?
        if (hadPath && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.05f)
        {
            hadPath = false;
            if (follow == FollowMode.None) SafeCall(OnArrived);
        }

        // Turning on the spot
        if (lookTarget.HasValue && agent.velocity.sqrMagnitude < 0.05f)
        {
            Vector3 dir = lookTarget.Value - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion goal = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, goal, settings.turnSpeed * Time.deltaTime);
                if (Quaternion.Angle(transform.rotation, goal) < 1f) lookTarget = null;
            }
            else lookTarget = null;
        }
    }

    // Runs a few times per second, not every frame, to keep path-finding cheap.
    void UpdateFollow()
    {
        if (follow == FollowMode.None || manualStop || state != BodyState.Walking) return;
        if (!ThingWorldPlayer.Exists || !agent.enabled || !agent.isOnNavMesh) return;

        Vector3 player = ThingWorldPlayer.FeetPosition;
        if (follow == FollowMode.FollowPlayer)
        {
            if ((player - transform.position).sqrMagnitude > 1.2f * 1.2f) SetDestinationSafe(player);
            else { agent.ResetPath(); hadPath = false; lookTarget = player; }
        }
        else
        {
            Vector3 away = transform.position - player;
            away.y = 0f;
            if (away.sqrMagnitude < 0.01f) away = transform.forward;
            if (away.magnitude < settings.wanderRadius)
                SetDestinationSafe(transform.position + away.normalized * 3f);
        }
    }

    void SetDestinationSafe(Vector3 place)
    {
        if (state != BodyState.Walking || !agent.enabled || !agent.isOnNavMesh) return;
        if (NavMesh.SamplePosition(place, out NavMeshHit hit, 3f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            hadPath = true;
        }
    }

    bool TryRandomPoint(Vector3 center, float radius, out Vector3 point)
    {
        for (int i = 0; i < 12; i++)
        {
            Vector2 r = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 candidate = center + new Vector3(r.x, 0f, r.y);
            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                point = hit.position;
                return true;
            }
        }
        point = center;
        return false;
    }

    void PlaceOnNavMesh(Vector3 near)
    {
        if (NavMesh.SamplePosition(near, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            body.isKinematic = true;
            transform.position = hit.position;
            agent.enabled = true;
            agent.Warp(hit.position);
            state = BodyState.Walking;
        }
        else if (ThingWorldManager.Instance != null && ThingWorldManager.Instance.TryGetSpawnPoint(out Vector3 spawn))
        {
            body.isKinematic = true;
            transform.position = spawn;
            agent.enabled = true;
            agent.Warp(spawn);
            state = BodyState.Walking;
        }
        else
        {
            agent.enabled = false;
            Warn("There is no walkable ground (NavMesh) here, so I can't walk. Is there a ThingWorldManager / NavMeshSurface in the scene?");
        }
    }

    // ---------------- grabbing & falling ----------------

    void HandleGrabbed(SelectEnterEventArgs args)
    {
        state = BodyState.Held;
        if (agent.enabled) agent.enabled = false;
        lastHeldPosition = transform.position;
        heldVelocity = Vector3.zero;
        SafeCall(OnGrabbed);
    }

    void HandleReleased(SelectExitEventArgs args)
    {
        if (grab.isSelected) return; // still held by the other hand

        // The toolkit has just restored our kinematic flag. Make the body physical *now*,
        // so the toolkit's own throw (applied later this frame) can give us velocity.
        state = BodyState.Falling;
        body.isKinematic = false;
        body.constraints = RigidbodyConstraints.FreezeRotation;
        fallStartTime = Time.time;
        restingTime = 0f;
        StartCoroutine(ReleaseRoutine());
    }

    IEnumerator ReleaseRoutine()
    {
        // Wait one frame so the throw velocity has been applied, then report it.
        yield return null;
        if (state != BodyState.Falling) yield break;
        Vector3 v = body.linearVelocity;
        if (v.sqrMagnitude < 0.01f && heldVelocity.sqrMagnitude > 0.25f)
        {
            v = heldVelocity; // fallback if the toolkit didn't throw
            body.linearVelocity = v;
        }
        body.linearVelocity = Vector3.ClampMagnitude(body.linearVelocity, 15f);
        SafeCall(() => OnReleased(v.magnitude));
    }

    void UpdateHeld()
    {
        float dt = Mathf.Max(Time.deltaTime, 0.0001f);
        Vector3 v = (transform.position - lastHeldPosition) / dt;
        heldVelocity = Vector3.Lerp(heldVelocity, v, 0.5f);
        lastHeldPosition = transform.position;
    }

    void UpdateFalling()
    {
        // Slowly turn upright while flying.
        Vector3 fwd = transform.forward; fwd.y = 0f;
        if (fwd.sqrMagnitude < 0.001f) fwd = Vector3.forward;
        body.MoveRotation(Quaternion.Slerp(body.rotation, Quaternion.LookRotation(fwd), Time.fixedDeltaTime * 4f));

        if (transform.position.y < KillY)
        {
            Respawn();
            return;
        }

        bool slow = body.linearVelocity.sqrMagnitude < 0.25f;
        bool grounded = Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, 0.5f, ~0, QueryTriggerInteraction.Ignore);
        restingTime = (slow && grounded) ? restingTime + Time.fixedDeltaTime : 0f;

        if (restingTime > 0.4f || Time.time - fallStartTime > 8f) Land();
    }

    void Land()
    {
        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.constraints = RigidbodyConstraints.None;
        body.isKinematic = true;
        Vector3 fwd = transform.forward; fwd.y = 0f;
        if (fwd.sqrMagnitude > 0.001f) transform.rotation = Quaternion.LookRotation(fwd);

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 3f, NavMesh.AllAreas))
        {
            PlaceOnNavMesh(hit.position);
            SafeCall(OnLanded);
        }
        else Respawn();
    }

    void Respawn()
    {
        body.linearVelocity = Vector3.zero;
        body.constraints = RigidbodyConstraints.None;
        body.isKinematic = true;
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        PlaceOnNavMesh(homePosition);
        SafeCall(OnLanded);
    }

    // ---------------- sensing ----------------

    void SenseNeighbors()
    {
        float r2 = settings.neighborRadius * settings.neighborRadius;
        foreach (Thing other in All.ToArray())
        {
            if (other == null || other == this) continue;
            bool close = (other.transform.position - transform.position).sqrMagnitude < r2;
            bool known = neighbors.Contains(other);
            if (close && !known)
            {
                neighbors.Add(other);
                SafeCall(() => OnMeetSomeone(other));
            }
            else if (!close && known)
            {
                neighbors.Remove(other);
                SafeCall(() => OnLeaveSomeone(other));
            }
        }
    }

    void SensePlayer()
    {
        if (!ThingWorldPlayer.Exists) return;
        float d = ThingWorldPlayer.DistanceTo(transform.position);
        if (!playerNear && d < settings.playerNearRadius)
        {
            playerNear = true;
            SafeCall(OnPlayerNear);
        }
        else if (playerNear && d > settings.playerNearRadius * 1.25f) // a little extra so it doesn't flicker
        {
            playerNear = false;
            SafeCall(OnPlayerLeave);
        }
    }

    void HandleSunrise() => SafeCall(OnSunrise);
    void HandleSunset() => SafeCall(OnSunset);

    // ---------------- animation ----------------

    void UpdateAnimator()
    {
        if (animator == null) return;
        float speed = (state == BodyState.Walking && agent.enabled) ? agent.velocity.magnitude : 0f;
        if (animatorParams.Contains("Speed")) animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        if (animatorParams.Contains("Grabbed")) animator.SetBool("Grabbed", state != BodyState.Walking);
    }

    void SetAnimatorTrigger(string name)
    {
        if (animatorParams.Contains(name)) animator.SetTrigger(name);
    }

    void MeasureHeight()
    {
        Bounds b = new Bounds(transform.position, Vector3.zero);
        bool any = false;
        foreach (Renderer r in bodyRenderers)
        {
            if (r is ParticleSystemRenderer) continue;
            if (!any) { b = r.bounds; any = true; } else b.Encapsulate(r.bounds);
        }
        baseHeight = any ? Mathf.Max(0.2f, b.max.y - transform.position.y) : capsule.height * transform.lossyScale.y;
        bodyHeight = baseHeight;
    }

    // ---------------- helpers ----------------

    IEnumerator AfterRoutine(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        SafeCall(action);
    }

    IEnumerator RepeatRoutine(float seconds, Action action)
    {
        var wait = new WaitForSeconds(seconds);
        while (true)
        {
            yield return wait;
            SafeCall(action);
        }
    }

    /// <summary>Runs student code so that one mistake can't crash the whole world.</summary>
    void SafeCall(Action action)
    {
        if (action == null) return;
        try { action(); }
        catch (Exception e) { Debug.LogError($"[{ThingName}] {e.GetType().Name}: {e.Message}\n{e.StackTrace}", this); }
    }

    void Warn(string message) => Debug.LogWarning($"[{ThingName}] {message}", this);

    void CheckForHiddenUnityMethods()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
        string[] reserved = { "Awake", "Start", "Update", "FixedUpdate", "OnEnable", "OnDisable", "OnDestroy" };
        for (Type t = GetType(); t != null && t != typeof(Thing); t = t.BaseType)
        {
            foreach (string m in reserved)
            {
                if (t.GetMethod(m, flags) != null)
                {
                    Debug.LogError($"[{ThingName}] Please don't write your own '{m}()' — it switches off Thing World. " +
                                   $"Use Setup(), OnBorn() or EveryFrame() instead.", this);
                }
            }
        }
    }
}
