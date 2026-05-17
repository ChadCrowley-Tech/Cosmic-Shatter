using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Creates a global version of this script
    public static AudioManager Instance;

    [Header("The Speakers")]
    // Speaker for sound effects
    public AudioSource sfxSource;
    // Speaker for background music
    public AudioSource bgmSource;  
    // Speaker for the ship thruster
    public AudioSource engineSource;

    [Header("Player Sounds")]
    // Holds the player laser sound
    public AudioClip laserSound;
    // Holds the player explosion sound
    public AudioClip playerExplosionSound;

    [Header("Enemy Sounds")]
    // Holds the UFO laser sound
    public AudioClip ufoLaserSound;
    // Holds the UFO death sound 
    public AudioClip ufoExplosionSound;

    [Header("Environment Sounds")]
    // Holds the large asteroid explosion sound
    public AudioClip bigExplosionSound;
    // Holds the small asteroid explosion sound
    public AudioClip smallExplosionSound;

    void Awake()
    {
        // Sets the global instance
        Instance = this;
    }

    void Start()
    {
        // Starts the background music
        bgmSource.Play();
    }

    // Engine sound logic
    public void PlayEngine()
    {
        // Plays engine sound if not already playing
        if (!engineSource.isPlaying) engineSource.Play();
    }

    public void StopEngine()
    {
        // Stops the engine sound
        if (engineSource.isPlaying) engineSource.Stop();
    }

    // One time sound effect logic
    public void PlayLaser() { sfxSource.PlayOneShot(laserSound); }
    public void PlayPlayerExplosion() { sfxSource.PlayOneShot(playerExplosionSound); }
    
    public void PlayUFOLaser() { sfxSource.PlayOneShot(ufoLaserSound); }
    public void PlayUFOExplosion() { sfxSource.PlayOneShot(ufoExplosionSound); }
    
    public void PlayBigExplosion() { sfxSource.PlayOneShot(bigExplosionSound); }
    public void PlaySmallExplosion() { sfxSource.PlayOneShot(smallExplosionSound); }
}
