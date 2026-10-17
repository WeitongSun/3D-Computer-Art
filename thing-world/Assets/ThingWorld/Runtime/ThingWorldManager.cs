using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// One per scene. Makes sure there is walkable ground (NavMesh) and brings every
/// student's Thing into the world.
///
/// - Every prefab in any "Resources/Things" folder is spawned once (unless it is already in the scene).
/// - If the NavMeshSurface has not been baked, it is baked when the scene starts.
/// </summary>
[DefaultExecutionOrder(-100)]
public class ThingWorldManager : MonoBehaviour
{
    public static ThingWorldManager Instance { get; private set; }

    [Tooltip("Spawn every prefab found in Resources/Things folders.")]
    public bool spawnAllThings = true;

    [Tooltip("Extra Things to spawn (for example test dummies). Leave empty in the final world.")]
    public List<GameObject> extraThings = new List<GameObject>();

    [Tooltip("Things appear at random walkable points within this distance of this object.")]
    public float spawnRadius = 10f;

    [Tooltip("Optional: exact spawn points. If set, Things are placed on these instead of random points.")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Tooltip("The walkable ground. If empty, the first NavMeshSurface in the scene is used.")]
    public NavMeshSurface navMeshSurface;

    int nextSpawnPoint;

    void Awake()
    {
        Instance = this;

        if (navMeshSurface == null) navMeshSurface = FindFirstObjectByType<NavMeshSurface>();
        if (navMeshSurface != null && navMeshSurface.navMeshData == null)
        {
            // Things and the player's body are not ground: leave them out of the bake.
            foreach (Thing t in FindObjectsByType<Thing>(FindObjectsSortMode.None)) IgnoreForNavMesh(t.gameObject);
            foreach (CharacterController c in FindObjectsByType<CharacterController>(FindObjectsSortMode.None)) IgnoreForNavMesh(c.gameObject);
            navMeshSurface.BuildNavMesh();
        }
        else if (navMeshSurface == null)
        {
            Debug.LogWarning("[Thing World] No NavMeshSurface in the scene — Things will not be able to walk.");
        }
    }

    void Start()
    {
        var alreadyHere = new HashSet<string>();
        foreach (Thing t in FindObjectsByType<Thing>(FindObjectsSortMode.None)) alreadyHere.Add(t.GetType().Name);

        var toSpawn = new List<GameObject>(extraThings);
        if (spawnAllThings) toSpawn.AddRange(Resources.LoadAll<GameObject>("Things"));

        foreach (GameObject prefab in toSpawn)
        {
            if (prefab == null) continue;
            Thing thing = prefab.GetComponent<Thing>();
            if (thing == null)
            {
                Debug.LogWarning($"[Thing World] '{prefab.name}' is in a Things folder but has no Thing script on it. Skipped.", prefab);
                continue;
            }
            string id = thing.GetType().Name;
            if (alreadyHere.Contains(id) && !extraThings.Contains(prefab)) continue;
            alreadyHere.Add(id);

            if (!TryGetSpawnPoint(out Vector3 p)) p = transform.position;
            Quaternion facing = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            GameObject go = Instantiate(prefab, p, facing);
            go.name = prefab.name;
        }
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    static void IgnoreForNavMesh(GameObject go)
    {
        var m = go.GetComponent<NavMeshModifier>();
        if (m == null) m = go.AddComponent<NavMeshModifier>();
        m.ignoreFromBuild = true;
    }

    /// <summary>Find a walkable spawn point.</summary>
    public bool TryGetSpawnPoint(out Vector3 point)
    {
        if (spawnPoints.Count > 0)
        {
            Transform sp = spawnPoints[nextSpawnPoint++ % spawnPoints.Count];
            if (sp != null && NavMesh.SamplePosition(sp.position, out NavMeshHit h, 3f, NavMesh.AllAreas))
            {
                point = h.position;
                return true;
            }
        }
        for (int i = 0; i < 30; i++)
        {
            Vector2 r = Random.insideUnitCircle * spawnRadius;
            Vector3 c = transform.position + new Vector3(r.x, 0f, r.y);
            if (NavMesh.SamplePosition(c, out NavMeshHit hit, 3f, NavMesh.AllAreas))
            {
                point = hit.position;
                return true;
            }
        }
        point = transform.position;
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.6f, 0.1f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
