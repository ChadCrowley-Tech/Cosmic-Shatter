using UnityEngine;

public class UFO : MonoBehaviour
{
    // UFO speed
    public float speed = 3f;
    // Laser object for UFO
    public GameObject enemyLaserPrefab;
    // Holds the explosion graphic
    public GameObject explosionPrefab;
    // UFO fire rate
    public float fireRate = 2f; 
    // Amount of points user gains for destroying UFO 
    public int scoreValue = 200;

    private Transform player;

    void Start()
    {
        // Start the shooting timer
        InvokeRepeating("Shoot", 1f, fireRate);

        // Destroy UFO after 12 seconds to prevent infinite flying
        Destroy(gameObject, 12f);
    }

    void Update()
    {
        // Moves UFO forward 
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    void Shoot()
    {
        // Finds the player right before shooting in case of respawn
        PlayerController p = FindAnyObjectByType<PlayerController>();
        
        if (p != null) 
        {
            player = p.transform;
            
            // Calculates the exact angle to point the laser at the player
            Vector2 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Instantiate(enemyLaserPrefab, transform.position, rotation);

            // Play UFO laser sound
            AudioManager.Instance.PlayUFOLaser();

        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Checks if hit by a player laser
        if (hitInfo.name.Contains("Laser") && !hitInfo.name.Contains("Enemy")) 
        {
            // Deletes the player laser
            Destroy(hitInfo.gameObject); 

            // Adds points to the total score
            GameManager.Instance.AddScore(scoreValue);

            // Spawns the explosion graphic 
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); 

            // Play UFO explosion sound
            AudioManager.Instance.PlayUFOExplosion();
            
            // Medium camera shake (0.2 seconds long, 0.25 power)
            if (CameraShake.Instance != null) CameraShake.Instance.Shake(0.2f, 0.25f);

            // Deletes the UFO
            Destroy(gameObject);
        }

    }
}
