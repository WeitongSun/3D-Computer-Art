using System.Collections;
using UnityEngine;

/// <summary>
/// THING WORLD — moves an object between a "closed" and an "open" pose. Doors, lifts, drawers, lids.
/// Put it on the part that moves (not on the whole building). Set how far it moves and turns,
/// then call Open / Close / Toggle from a World Interactable.
/// </summary>
[DisallowMultipleComponent]
public class SimpleMover : MonoBehaviour
{
    [Tooltip("How far it moves when open (meters, in its parent's space).")]
    public Vector3 moveBy = Vector3.zero;

    [Tooltip("How much it turns when open (degrees). A door: (0, 90, 0).")]
    public Vector3 rotateBy = new Vector3(0f, 90f, 0f);

    [Tooltip("Seconds to open or close.")]
    public float duration = 1f;

    public bool startOpen = false;

    [Tooltip("Optional sound when it moves.")]
    public AudioClip sound;

    public bool IsOpen { get; private set; }

    Vector3 closedPosition;
    Quaternion closedRotation;
    Coroutine running;
    AudioSource audioSource;

    void Awake()
    {
        closedPosition = transform.localPosition;
        closedRotation = transform.localRotation;
        if (startOpen) Snap(true);
    }

    public void Open() => MoveTo(true);
    public void Close() => MoveTo(false);
    public void Toggle() => MoveTo(!IsOpen);

    void MoveTo(bool open)
    {
        if (open == IsOpen && running == null) return;
        IsOpen = open;
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(Animate(open));
        if (sound != null)
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 1f;
            }
            audioSource.PlayOneShot(sound);
        }
    }

    void Snap(bool open)
    {
        IsOpen = open;
        transform.localPosition = open ? closedPosition + moveBy : closedPosition;
        transform.localRotation = open ? closedRotation * Quaternion.Euler(rotateBy) : closedRotation;
    }

    IEnumerator Animate(bool open)
    {
        Vector3 fromPos = transform.localPosition;
        Quaternion fromRot = transform.localRotation;
        Vector3 toPos = open ? closedPosition + moveBy : closedPosition;
        Quaternion toRot = open ? closedRotation * Quaternion.Euler(rotateBy) : closedRotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.05f, duration);
            float s = Mathf.SmoothStep(0f, 1f, t);
            transform.localPosition = Vector3.Lerp(fromPos, toPos, s);
            transform.localRotation = Quaternion.Slerp(fromRot, toRot, s);
            yield return null;
        }
        running = null;
    }
}
