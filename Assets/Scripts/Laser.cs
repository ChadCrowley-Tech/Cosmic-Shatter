using UnityEngine;

public class Laser : MonoBehaviour
{
    // Player laser speed
    public float speed = 10f;
    // Time before player laser is deleted
    public float lifetime = 3f;

    void Start()
    {
        // Deletes the laser after a set time to prevent infinite flying
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Moves the laser forward based on its current rotation
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

}
