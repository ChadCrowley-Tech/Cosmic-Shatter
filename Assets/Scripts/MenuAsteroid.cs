using UnityEngine;

public class MenuAsteroid : MonoBehaviour
{
    private float moveSpeed;
    private float rotationSpeed;

    // Straight path for the asteroid
    private Vector3 moveDirection; 
    
    // Screen boundaries for wrapping 
    private float screenTop = 6f;
    private float screenBottom = -6f;
    private float screenLeft = -10f;
    private float screenRight = 10f;

    void Start()
    {
        // Sets random direction, spin, and speed for asteroids
        moveSpeed = Random.Range(1f, 3f);
        rotationSpeed = Random.Range(-40f, 40f);

        // Calculates a random angle to fly toward
        float randomAngle = Random.Range(0f, 360f);
        moveDirection = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0).normalized;

    }

    void Update()
    {
        // Move in a straight line
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        
        // Tumble/Spin the asteroid graphic on the Z axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // Keeps asteroid on the screen
        Vector2 pos = transform.position;
        if (pos.x > screenRight) pos.x = screenLeft;
        if (pos.x < screenLeft) pos.x = screenRight;
        if (pos.y > screenTop) pos.y = screenBottom;
        if (pos.y < screenBottom) pos.y = screenTop;
        transform.position = pos;
    }
}
