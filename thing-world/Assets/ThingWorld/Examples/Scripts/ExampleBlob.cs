using UnityEngine;

/// <summary>
/// A simple test neighbor. Blobs appear in your test scene so your Thing has someone to meet.
/// Read this script as an example of how a Thing is written.
/// </summary>
public class ExampleBlob : Thing
{
    string[] greetings = { "hi!", "oh, hello", "nice day", "who are you?", "..." };

    protected override void Setup()
    {
        settings.walkSpeed = 0.8f;
        settings.wanderRadius = 5f;
        settings.myColor = Color.HSVToRGB(Random.value, 0.6f, 1f);
    }

    protected override void OnBorn()
    {
        SetColor(settings.myColor);
    }

    protected override void OnMeetSomeone(Thing other)
    {
        LookAt(other.transform.position);
        Speak(greetings[Random.Range(0, greetings.Length)]);
    }

    protected override void OnNeighborSparks(Thing who)
    {
        Spark(); // sparks are contagious
    }

    protected override void OnGrabbed()
    {
        Speak("whoa!");
        SetColor(Color.white);
    }

    protected override void OnReleased(float throwSpeed)
    {
        if (throwSpeed > 3f) Speak("wheeee!");
        SetColor(settings.myColor);
    }

    protected override void OnSunset()
    {
        SetColor(settings.myColor * 0.4f);
    }

    protected override void OnSunrise()
    {
        SetColor(settings.myColor);
    }
}
