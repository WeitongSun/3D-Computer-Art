using UnityEngine;

/// <summary>
/// Shared settings for the whole class. Lives at Assets/ThingWorld/Resources/ThingWorldConfig.asset
/// and is created by "Thing World → Instructor → Set Up Project".
/// </summary>
[CreateAssetMenu(menuName = "Thing World/Config", fileName = "ThingWorldConfig")]
public class ThingWorldConfig : ScriptableObject
{
    public const string ResourcePath = "ThingWorldConfig";

    [Header("Assets made by the setup tool")]
    public Material sparkMaterial;
    public RuntimeAnimatorController baseAnimator;

    [Header("Character budget (checked by 'Check My Work')")]
    public int characterMaxTriangles = 15000;
    public int characterMaxMaterials = 4;
    public float characterMinHeight = 0.3f;
    public float characterMaxHeight = 3f;

    [Header("City piece budget (checked by 'Check My Work')")]
    public int piecesMaxTrianglesPerStudent = 40000;
    public int pieceMaxMaterials = 4;
    public float pieceMaxSize = 30f;

    [Header("Textures")]
    public int maxTextureSize = 2048;
    [Tooltip("Sky panoramas may be bigger than other textures.")]
    public int skyMaxTextureSize = 4096;

    [Header("World building budgets (checked by 'Check My Work')")]
    public int groundMaxTriangles = 100000;
    [Tooltip("Whole ground, meters. Keep the walkable part about 40 × 40 m so Things meet each other.")]
    public float groundMaxSize = 60f;
    public float walkableMaxSize = 40f;
    [Tooltip("Point/spot lights per Lighting coordinator. Lights are expensive on Quest.")]
    public int maxLampsPerStudent = 4;
    public int particleMaxPerSystem = 500;
    public int particleMaxPerStudent = 1500;
    [Tooltip("Looping sounds per Sound coordinator.")]
    public int maxLoopingSoundsPerStudent = 8;

    static ThingWorldConfig cached;

    public static ThingWorldConfig Load()
    {
        if (cached == null) cached = Resources.Load<ThingWorldConfig>(ResourcePath);
        return cached;
    }
}
