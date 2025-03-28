using UnityEngine;

public class CarDriftSound : MonoBehaviour
{
    public AudioSource driftAudio; // Assign in Inspector
    public float driftThreshold = 1.5f; // Minimum lateral speed to start sound
    public float maxDriftSpeed = 5f; // Maximum speed for full volume/pitch
    public float fadeSpeed = 5f; // How fast volume fades
    public float minPitch = 1.3f; // Base pitch
    public float maxPitch = 1.8f; // Max pitch variation

    private Rigidbody2D rb;
    private float targetVolume = 0f;
    private bool isDrifting = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        driftAudio.volume = 0f;
        driftAudio.loop = true;
        driftAudio.Play();
    }

    void Update()
    {
        float lateralSpeed = Mathf.Abs(Vector2.Dot(rb.linearVelocity, transform.right)); // Get sideways movement

        // Check if drifting
        if (lateralSpeed > driftThreshold)
        {
            targetVolume = Mathf.Clamp(lateralSpeed / maxDriftSpeed, 0.2f, 1f); // Scale volume smoothly
            driftAudio.pitch = Mathf.Lerp(minPitch, maxPitch, targetVolume); // Adjust pitch
            isDrifting = true;
        }
        else
        {
            targetVolume = 0f;
            isDrifting = false;
        }

        // Smoothly transition volume
        driftAudio.volume = Mathf.Lerp(driftAudio.volume, targetVolume, Time.deltaTime * fadeSpeed);

        // Prevent echo by stopping sound when fully faded out
        if (!isDrifting && driftAudio.volume < 0.05f)
            driftAudio.Stop();
        else if (isDrifting && !driftAudio.isPlaying)
            driftAudio.Play();
    }
}