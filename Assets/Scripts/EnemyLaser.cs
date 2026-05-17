using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    // Moves slightly slower than player lasers
    public float speed = 7f; 
    // Time before laser is deleted
    public float lifetime = 3f;

    void Start()
    {
        // Deletes the laser after a set time to prevent infinite flying
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Moves the laser forward based on its current location
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
