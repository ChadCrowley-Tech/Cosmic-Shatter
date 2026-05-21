using UnityEngine;
using UnityEngine.EventSystems; // Added to prevent screen taps from double firing

public class PlayerController : MonoBehaviour
{
    // Visual for the engine thruster
    public ParticleSystem engineFire;

    // Variables for player movement
    public float thrustSpeed = 5f;
    public float rotationSpeed = 200f;

    // Holds the explosion graphic
    public GameObject explosionPrefab;

    // Holds the laser graphic
    public GameObject laserPrefab; 
    public Transform firePoint;

    // Screen borders for wrapping
    private float screenTop;
    private float screenBottom;
    private float screenLeft;
    private float screenRight;

    // How long the player is safe after spawning
    public float invincibilityTime = 3f;

    // Tracks whether player can take damage
    private bool isInvincible = true; 

    // Controls the player graphic
    private SpriteRenderer spriteRenderer;

    private MobileButton btnLeft;
    private MobileButton btnRight;
    private MobileButton btnThrust;
    private MobileButton btnThrustAlt;
    private MobileButton btnFire;
    private bool mobileFireReady = true;
    private bool isMobileHardware;

    void Start()
    {
        // Finds the edges of the camera view
        Camera cam = Camera.main;
        
        screenTop = cam.orthographicSize;
        screenBottom = -screenTop;
        
        screenRight = cam.orthographicSize * cam.aspect;
        screenLeft = -screenRight;

        // Grab the sprite renderer to make it blink
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Turns off safety after a set time
        Invoke("TurnOffInvincibility", invincibilityTime);

        // Hardware check
        isMobileHardware = Application.isMobilePlatform || SystemInfo.deviceType == DeviceType.Handheld;

        GameObject leftObj = GameObject.Find("Btn_Left");
        if (leftObj != null) btnLeft = leftObj.GetComponent<MobileButton>();

        GameObject rightObj = GameObject.Find("Btn_Right");
        if (rightObj != null) btnRight = rightObj.GetComponent<MobileButton>();

        GameObject thrustObj = GameObject.Find("Btn_Thrust");
        if (thrustObj != null) btnThrust = thrustObj.GetComponent<MobileButton>();

        GameObject thrustAltObj = GameObject.Find("Btn_Thrust_Alt");
        if (thrustAltObj != null) btnThrustAlt = thrustAltObj.GetComponent<MobileButton>();

        GameObject fireObj = GameObject.Find("Btn_Fire");
        if (fireObj != null) btnFire = fireObj.GetComponent<MobileButton>();

        // Platform detection
        // Find the container holding all the mobile buttons
        GameObject mobileUI = GameObject.Find("MobileUI");
        if (mobileUI != null)
        {
            // Check to see if game is running on PC
            if (!Application.isMobilePlatform)
            {
                // Turn off mobile buttons
                mobileUI.SetActive(false);
            }
        }

    }

    void Update()
    {
        // HYBRID MOVEMENT (PC and MOBILE)
        // Checks if player is pressing movement keys
        float thrustInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Overrides PC input if a mobile UI button is currently being pressed
        if (btnLeft != null && btnLeft.isPressed) turnInput = -1f;
        if (btnRight != null && btnRight.isPressed) turnInput = 1f;
        if (btnThrust != null && btnThrust.isPressed || btnThrustAlt != null && btnThrustAlt.isPressed) thrustInput = 1f;

        // Turns the ship based on input
        transform.Rotate(Vector3.forward, -turnInput * rotationSpeed * Time.deltaTime);

        // Handle thrust (only forward, no reverse)
        if (thrustInput > 0)
        {
            // Moves the spaceship forward
            transform.Translate(Vector3.up * thrustInput * thrustSpeed * Time.deltaTime);

            // Starts the engine fire graphic
            if (!engineFire.isPlaying) engineFire.Play();
            // Starts the engine sound         
            AudioManager.Instance.PlayEngine(); 

        }

        else 
        {
            
            // Stops the engine fire graphic 
            if (engineFire.isPlaying) engineFire.Stop();
            // Stops the engine sound
            AudioManager.Instance.StopEngine(); 

        }

        // Keeps spaceship inside the screen
        WrapScreen();

        // HYBRID SHOOTING (PC and MOBILE)
        // PC Click / Screen Tap 
        if (!isMobileHardware && Input.GetMouseButtonDown(0)) 
        {
            if (EventSystem.current != null && !EventSystem.current.IsPointerOverGameObject())
            {
                Shoot();
            }
        }

        // Mobile Fire Button
        if (btnFire != null && btnFire.isPressed)
        {
            if (mobileFireReady)
            {
                Shoot();
                // Locks the laser until the thumb is lifted
                mobileFireReady = false;
            }
        }
        else
        {
            // Unlocks when the thumb leaves the screen 
            mobileFireReady = true; 

        }

        // Checks for hyperspace buttons (Left Shift/Right Mouse Button)
        if (Input.GetKeyDown(KeyCode.LeftShift) || (!isMobileHardware && Input.GetMouseButtonDown(1)))
        {
            Hyperspace();
        }

        // Handles safety blinking
        if (isInvincible)
        {
            // Turns graphic on and off (if it's greater than 0.5, turn the graphic on. If not, turn it off)
            spriteRenderer.enabled = Mathf.PingPong(Time.time * 5f, 1f) > 0.5f;
        }

    }

    void Shoot()
    {
        // Spawns a new laser
        Instantiate(laserPrefab, firePoint.position, firePoint.rotation);

        // Plays the laser sound
        AudioManager.Instance.PlayLaser();
    
    }

    void Hyperspace()
    {
        // Pick a random X and Y inside the screen edges
        float randomX = Random.Range(screenLeft, screenRight);
        float randomY = Random.Range(screenBottom, screenTop);

        // Move the ship to that new random x,y coordinate
        transform.position = new Vector3(randomX, randomY, transform.position.z);
    }

    void WrapScreen()
    {
        // Grab the current position of the ship
        Vector3 newPosition = transform.position;

        // Move ship to opposite side when it goes off screen horizontally
        if (newPosition.x > screenRight)
        {
            newPosition.x = screenLeft;
        }
        else if (newPosition.x < screenLeft)
        {
            newPosition.x = screenRight;
        }

        // Move ship to opposite side when it goes off screen vertically
        if (newPosition.y > screenTop)
        {
            newPosition.y = screenBottom;
        }
        else if (newPosition.y < screenBottom)
        {
            newPosition.y = screenTop;
        }

        // Apply the new position back to the ship
        transform.position = newPosition;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {

        // Ignores hits if player is safe/blinking
        if (isInvincible) return;

        // Checks if the the hit object is dangerous to the player
        if (hitInfo.name.Contains("Asteroid") || hitInfo.name.Contains("UFO") || hitInfo.name.Contains("EnemyLaser"))
        {
           
            // Turns off engine sound 
            AudioManager.Instance.StopEngine(); 
            // Play death sound
            AudioManager.Instance.PlayPlayerExplosion(); 
            // Big camera shake (0.4 seconds long, 0.5 power)
            if (CameraShake.Instance != null) CameraShake.Instance.Shake(0.4f, 0.5f);

            // Tells the GameManager the player died
            GameManager.Instance.PlayerDied();
            // Spawns the explosion graphic
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            // Deletes the player ship
            Destroy(gameObject);
            
        }
    }
    void TurnOffInvincibility()
    {
        // Makes the player able to take damage again
        isInvincible = false;
        
        // Makes the ship visible when blinking stops
        spriteRenderer.enabled = true; 
    }

}
