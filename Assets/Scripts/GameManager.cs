using UnityEngine;
using TMPro; // Talk to the TextMeshPro UI
using UnityEngine.SceneManagement; // Reload the level

public class GameManager : MonoBehaviour
{
    // Creates a global version of this script
    public static GameManager Instance;

    public SecureInt score = 0;

    // Lives System Variables
    public SecureInt lives = 3;

     // Slot for score text
    public TextMeshProUGUI scoreText; 

    // Slot for the Lives text
    public TextMeshProUGUI livesText;

    // The blueprint to build the spaceship
    public GameObject playerPrefab;

    // Slot for the Game Over screen
    public GameObject gameOverUI;

    // Slot for the leaderboard text
    public TextMeshProUGUI highScoreText;

    // UI Elements for the Initials
    public TMP_InputField initialsInput;
    public GameObject submitButton;
    public GameObject restartButton;

    // Wave System Variables
    public int level = 1;
    public TextMeshProUGUI waveText;
    // Blueprint for asteroids
    public GameObject asteroidPrefab; 

    // next wave banner
    public GameObject waveBannerUI; 
    public TextMeshProUGUI waveBannerText;
    // Blueprint for UFO
    public GameObject ufoPrefab;
    public float ufoSpawnTimer = 20f;

    private float screenTop;
    private float screenBottom;
    private float screenLeft;
    private float screenRight;

    void Awake()
    {
        // Sets the global instance to this script
        Instance = this;
    }

    void Start()
    {
        // Calculate screen edges
        Camera cam = Camera.main;
        screenTop = cam.orthographicSize;
        screenBottom = -screenTop;
        screenRight = cam.orthographicSize * cam.aspect;
        screenLeft = -screenRight;

        // Start the first wave
        UpdateWaveUI();
        // Show the banner on Wave 1, then wait 3 seconds to spawn
        ShowWaveBanner();
        Invoke("StartWave", 3f);

        InvokeRepeating("SpawnUFO", ufoSpawnTimer, ufoSpawnTimer);

    }

    // Adds points to score
    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "SCORE: " + score;
    }

    // The death logic
    public void PlayerDied()
    {
        // Subtract one life
        lives--; 

        // Update the UI when a life is lost
        UpdateLivesUI();

        if (lives > 0)
        {
            // Respawn after a 3-second delay
            Invoke("Respawn", 3f); 
        }
        else
        {
            // Trigger game over
            GameOver();
        }
    }

    // Keep the text synced with the variable
    void UpdateLivesUI()
    {
        livesText.text = "LIVES: " + lives;
    }

    void Respawn()
    {
        // Spawns new player at the center of screen  
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }

    void GameOver()
    {        
        gameOverUI.SetActive(true);
        
        // Load leaderboard from the encrypted vault
        LeaderboardData board = SecureSaveSystem.LoadLeaderboard();
        // The tenth place score
        int lowestScore = board.scores[9]; 

        if (score > lowestScore)
        {
            initialsInput.gameObject.SetActive(true);
            submitButton.SetActive(true);
            restartButton.SetActive(false);
            highScoreText.text = "NEW HIGH SCORE!\nENTER INITIALS:";
        }
        else
        {
            initialsInput.gameObject.SetActive(false);
            submitButton.SetActive(false);
            restartButton.SetActive(true);
            SaveAndDisplayScores("---"); 
        }
    }

    // Triggered by the Submit Button
    public void SubmitInitials()
    {
        // Grab the text, make it uppercase
        string initials = initialsInput.text.ToUpper();
        
        // If initials are left blank, give them "AAA"
        if (string.IsNullOrEmpty(initials)) initials = "AAA";

        // Hide the input, show the restart button
        initialsInput.gameObject.SetActive(false);
        submitButton.SetActive(false);
        restartButton.SetActive(true);

        // Save the new score
        SaveAndDisplayScores(initials);
    }

    void SaveAndDisplayScores(string initials)
    {
        // Load leaderboard data
        LeaderboardData board = SecureSaveSystem.LoadLeaderboard();

        // Find where the new score belongs
        for(int i = 0; i < 10; i++)
        {
            if (score > board.scores[i])
            {
                // Shift lower scores down
                for (int j = 9; j > i; j--)
                {
                    board.scores[j] = board.scores[j - 1];
                    board.names[j] = board.names[j - 1];
                }
                
                // Insert the new score
                board.scores[i] = score;
                board.names[i] = initials;
                break; 
            }
        }

        // Save the modified board back to the hard drive securely
        SecureSaveSystem.SaveLeaderboard(board);

        // Build text for the UI
        string leaderboard = "TOP 10 SCORES\n\n";
        for(int i = 0; i < 10; i++)
        {
            if (board.scores[i] > 0) 
            {
                leaderboard += (i + 1) + ". " + board.names[i] + " - " + board.scores[i] + "\n";
            }
        }
        
        highScoreText.text = leaderboard; 
        score = 0; 
    }

    // Triggered by the restart button
    public void RestartGame()
    {
        // Reload/reset active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Wave system logic
    public void CheckForNextWave()
    {
        // Count remaining asteroids
        Asteroid[] activeAsteroids = FindObjectsByType<Asteroid>
        (FindObjectsInactive.Exclude);

        // Check if screen is clear
        if (activeAsteroids.Length <= 1)
        {
            level++;
            UpdateWaveUI();

            // Show banner during transition.
            ShowWaveBanner();
            Invoke("StartWave", 3f);

        }
    }

    // Turns banner on and sets text
    void ShowWaveBanner()
    {
        waveBannerUI.SetActive(true);
        waveBannerText.text = "WAVE " + level;
    }

    void StartWave()
    {
        // Turn the banner off 
        waveBannerUI.SetActive(false);
        
        // Calculate how many asteroids to spawn
        // Wave 1 = 3 rocks, Wave 2 = 4 rocks, etc.
        int asteroidsToSpawn = level + 2;

        for (int i = 0; i < asteroidsToSpawn; i++)
        {
            Vector2 spawnPos = Vector2.zero;

            // Spawn on the Top/Bottom edges OR Left/Right edges
            if (Random.value > 0.5f)
            {
                spawnPos.x = Random.Range(screenLeft, screenRight);
                spawnPos.y = Random.value > 0.5f ? screenTop : screenBottom;
            }
            else
            {
                spawnPos.x = Random.value > 0.5f ? screenLeft : screenRight;
                spawnPos.y = Random.Range(screenBottom, screenTop);
            }

            Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
        }
    }

    void UpdateWaveUI()
    {
        waveText.text = "WAVE: " + level;
    }     

    void SpawnUFO()
    {
        // Only spawn if no UFO already exists
        if (FindAnyObjectByType<UFO>() == null)
        {
            Vector2 spawnPos = new Vector2(screenLeft - 1f, Random.Range(screenBottom, screenTop));
            // Point UFO right
            Quaternion rotation = Quaternion.Euler(0, 0, -90f); 
            
            // Chance to spawn on the right side pointing left
            if (Random.value > 0.5f)
            {
                spawnPos.x = screenRight + 1f;
                rotation = Quaternion.Euler(0, 0, 90f); 
            }

            Instantiate(ufoPrefab, spawnPos, rotation);
        }
    }

    void Update()
    {
        // Closes the game when the escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
