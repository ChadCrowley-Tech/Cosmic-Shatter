using UnityEngine;

public class Asteroid : MonoBehaviour
{   
    // The blueprint to spawn smaller rocks
    public GameObject asteroidPrefab; 

    // Starting sizes: 1 = large, 0.5 = medium, 0.25 = small
    public float size = 1f;  

    // The baseline thrust for a large asteroid         
    public float baseSpeed = 50f;     

    // Smallest size before breaking apart
    public float minSize = 0.4f;

    // Physics component to allow hits
    private Rigidbody2D rb;

    // Holds the explosion graphic
    public GameObject explosionPrefab;

    // Borders for screen wrapping
    private float screenTop;
    private float screenBottom;
    private float screenLeft;
    private float screenRight;

    void Start()
    {
        // Gets physics component
        rb = GetComponent<Rigidbody2D>();

        // Set the physical visual scale based on the size
        transform.localScale = new Vector3(size, size, 1f);

        // As asteroid size goes down, speed goes up.
        float currentSpeed = baseSpeed / size;

        // Pick a random direction
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        
        // Pushes the asteroid and give it a random spin
        rb.AddForce(randomDirection * currentSpeed);
        rb.AddTorque(Random.Range(-50f, 50f));

        // Calculate the screen edges
        Camera cam = Camera.main;
        screenTop = cam.orthographicSize;
        screenBottom = -screenTop;
        screenRight = cam.orthographicSize * cam.aspect;
        screenLeft = -screenRight;
    }

        void Update() 
    {
        // Keeps asteroid inside screen
        WrapScreen();
    }

    // The collision logic
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Checks if hit by a laser
        if (hitInfo.name.Contains("Laser")) 
        {

            // Spawns the explosion
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // Remove the explosion object after 2 seconds
            Destroy(explosion, 2f);

            
            // Checks if asteroid is large enough to split
            if (size > minSize)
            {
                // Plays large explosion sound
                AudioManager.Instance.PlayBigExplosion();

                // Shakes the camera
                if (CameraShake.Instance != null) CameraShake.Instance.Shake(0.1f, 0.15f);
                
            }
            else
            {
                // Plays small explosion sound
                AudioManager.Instance.PlaySmallExplosion();
            }
            
            // Destroys the laser
            Destroy(hitInfo.gameObject);

            // Tells the global GameManager to add 100 points.
            GameManager.Instance.AddScore(100);

            // Breaks into pieces if large enough
            if (size > minSize) 
            {
                Split();
                Split();
            }

            // Checks if current wave is clear
            GameManager.Instance.CheckForNextWave();

            // Deletes the asteroid
            Destroy(gameObject);
        }
    }

    void Split()
    {
        // Pick random position, slightly offset from center, so asteroids don't spawn inside each other
        Vector2 spawnPosition = (Vector2)transform.position + Random.insideUnitCircle * 0.5f;
        
        // Spawns the smaller asteroid
        GameObject newAsteroid = Instantiate(asteroidPrefab, spawnPosition, transform.rotation);
        
        // Cuts size of new asteroid by half
        Asteroid astScript = newAsteroid.GetComponent<Asteroid>();
        astScript.size = size * 0.5f;
    }

    void WrapScreen()
    {
        // Gets current location
        Vector3 newPosition = transform.position;

        // Moves asteroid to opposite side if it goes off screen horizantally
        if (newPosition.x > screenRight) newPosition.x = screenLeft;
        else if (newPosition.x < screenLeft) newPosition.x = screenRight;

        // Moves asteroid to opposite side if it goes off screen vertically
        if (newPosition.y > screenTop) newPosition.y = screenBottom;
        else if (newPosition.y < screenBottom) newPosition.y = screenTop;

        // Applies the new position
        transform.position = newPosition;
    }

}
