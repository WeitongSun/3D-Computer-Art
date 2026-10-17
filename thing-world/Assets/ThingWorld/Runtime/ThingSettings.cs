using UnityEngine;

/// <summary>
/// The personality knobs of a Thing. Change these inside Setup() in your own script.
/// Example:  settings.walkSpeed = 2f;
/// </summary>
[System.Serializable]
public class ThingSettings
{
    [Header("Moving")]
    [Tooltip("How fast you walk, in meters per second. A calm human walks about 1.2.")]
    public float walkSpeed = 1.2f;

    [Tooltip("How fast you turn, in degrees per second.")]
    public float turnSpeed = 240f;

    [Tooltip("If true, you pick a new random place to walk to every 'wanderInterval' seconds.")]
    public bool autoWander = true;

    [Tooltip("Seconds between new random destinations when autoWander is on.")]
    public float wanderInterval = 6f;

    [Tooltip("How far away (meters) a random destination can be.")]
    public float wanderRadius = 8f;

    [Header("Sensing")]
    [Tooltip("Another Thing closer than this (meters) counts as your neighbor.")]
    public float neighborRadius = 3f;

    [Tooltip("The player counts as 'near' when closer than this (meters).")]
    public float playerNearRadius = 2f;

    [Header("Body")]
    [Tooltip("Can the player pick you up?")]
    public bool canBeGrabbed = true;

    [Tooltip("Extra height (meters) above your head for speech bubbles.")]
    public float speechBubbleOffset = 0.3f;

    [Tooltip("Your signature color, used by Spark() when you don't give a color.")]
    public Color myColor = Color.white;
}
