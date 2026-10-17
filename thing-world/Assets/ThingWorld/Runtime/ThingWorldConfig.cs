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

    static ThingWorldConfig cached;

    public static ThingWorldConfig Load()
    {
        if (cached == null) cached = Resources.Load<ThingWorldConfig>(ResourcePath);
        return cached;
    }
}
