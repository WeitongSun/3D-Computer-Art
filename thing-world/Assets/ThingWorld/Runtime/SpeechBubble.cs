using TMPro;
using UnityEngine;

/// <summary>
/// The floating text above a Thing's head. Created automatically the first time a Thing calls Speak().
/// It always turns to face the player.
/// </summary>
public class SpeechBubble : MonoBehaviour
{
    TextMeshPro label;
    Thing owner;
    float hideAt;
    float popTime;

    internal static SpeechBubble Create(Thing owner)
    {
        var go = new GameObject("Speech Bubble");
        go.transform.SetParent(owner.transform, false);
        var bubble = go.AddComponent<SpeechBubble>();
        bubble.owner = owner;

        bubble.label = go.AddComponent<TextMeshPro>();
        bubble.label.fontSize = 2f;
        bubble.label.alignment = TextAlignmentOptions.Bottom;
        bubble.label.color = Color.white;
        bubble.label.rectTransform.sizeDelta = new Vector2(2.4f, 1f);
        bubble.label.text = "";
        if (bubble.label.font != null)
        {
            bubble.label.outlineWidth = 0.25f;
            bubble.label.outlineColor = new Color32(0, 0, 0, 255);
        }
        go.SetActive(false);
        return bubble;
    }

    internal void Show(string words, float seconds)
    {
        if (label.font == null)
        {
            Debug.LogWarning("[Thing World] Speech bubbles need TextMeshPro: Window → TextMeshPro → Import TMP Essential Resources.");
            return;
        }
        label.text = words;
        hideAt = Time.time + Mathf.Max(0.5f, seconds);
        popTime = 0f;
        gameObject.SetActive(true);
    }

    void LateUpdate()
    {
        if (owner == null) { Destroy(gameObject); return; }
        if (Time.time > hideAt) { gameObject.SetActive(false); return; }

        // Stay above the head in world space, even if the body is scaled or tilted.
        Vector3 parentScale = owner.transform.lossyScale;
        transform.position = owner.transform.position + Vector3.up * owner.BubbleHeight;

        // Little pop-in animation.
        popTime += Time.deltaTime;
        float pop = Mathf.SmoothStep(0.4f, 1f, Mathf.Clamp01(popTime / 0.15f));
        transform.localScale = new Vector3(pop / Mathf.Max(0.01f, parentScale.x),
                                           pop / Mathf.Max(0.01f, parentScale.y),
                                           pop / Mathf.Max(0.01f, parentScale.z));

        if (ThingWorldPlayer.Exists)
        {
            Vector3 look = transform.position - ThingWorldPlayer.HeadPosition;
            if (look.sqrMagnitude > 0.001f) transform.rotation = Quaternion.LookRotation(look);
        }
    }
}
