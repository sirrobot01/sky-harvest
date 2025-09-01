using UnityEngine;

public class ThunderCloud : MonoBehaviour
{
    [Header("Behavior")]
    public GameObject lightningBolt;      // Assign the child LightningBolt in Inspector
    public float minStrikeInterval = 2f;    // Minimum time between strikes
    public float maxStrikeInterval = 3f;    // Maximum time between strikes
    public float strikeDuration = 0.3f;     // How long the bolt stays active/flashing
    public float thunderVolume = 0.8f;    // Controls the volume of the thunder sound

    [Header("Sound")]
    // --- NEW --- This is the slot for your audio file.
    public AudioClip thunderSoundClip; 
    [Header("Movement")]
    public float scrollSpeed = 4f;
    public float offScreenDestroyX = -20f; // X position to destroy the object

    private float strikeTimer;
    private AudioSource localAudioSource; // A reference to AudioSource

    void Start()
    {
        // Get the AudioSource component from thunder prefab.
        localAudioSource = GetComponent<AudioSource>();
        if (localAudioSource != null)
        {
            localAudioSource.clip = thunderSoundClip;
            localAudioSource.volume = thunderVolume;
        }


        // Deactivate the lightning bolt at the start.
        if (lightningBolt != null)
        {
            lightningBolt.SetActive(false);
        }
        // Start with a random delay so multiple clouds don't sync up.
        strikeTimer = Random.Range(minStrikeInterval, maxStrikeInterval);
    }

    void Update()
    {
        if (GameManager.isLevelOver) return;

        // The cloud now continuously scrolls to the left.
        transform.position += Vector3.left * scrollSpeed * PlayerDragonController.scrollSpeedMultiplier * Time.deltaTime;

        // Strike timer logic remains the same.
        strikeTimer -= Time.deltaTime;
        if (strikeTimer <= 0)
        {
            Strike();
            strikeTimer = Random.Range(minStrikeInterval, maxStrikeInterval);
        }

        // Destroy the cloud when it goes far off-screen to the left.
        if (transform.position.x < offScreenDestroyX)
        {
            Destroy(gameObject);
        }
    }

    private void Strike()
    {
        if (lightningBolt != null)
        {
            lightningBolt.SetActive(true);

            // Play the sound from AudioSource.
            if (localAudioSource != null && localAudioSource.clip != null)
            {
                localAudioSource.Play();
            }
            // Hide after duration (flash effect).
            Invoke("HideBolt", strikeDuration);
        }
    }

    private void HideBolt()
    {
        if (lightningBolt != null)
        {
            lightningBolt.SetActive(false);
        }
    }
}
