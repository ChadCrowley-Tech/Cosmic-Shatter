using UnityEngine;
using System.Collections; // Required for Coroutines

public class CameraShake : MonoBehaviour
{
    // Creates a global version of the script
    public static CameraShake Instance;
    private Vector3 originalPos;

    void Awake()
    {
        // Sets the global instance
        Instance = this;
    }

    void Start()
    {
        // Saves the original camera location
        originalPos = transform.localPosition;
    }

    // Starts the camera shake with duration and strength
    public void Shake(float duration, float magnitude)
    {
        // Stop any current shakes before starting a new one
        StopAllCoroutines(); 
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    // Runs over time instead of instantly
    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // Pick a random direction to move
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            // Moves the camera
            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            
            // Wait for the next frame before looping
            yield return null; 
        }

        // Snap the camera back to the center
        transform.localPosition = originalPos; 
    }
}
