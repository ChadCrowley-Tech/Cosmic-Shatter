using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Leaderboard UI")]

    // Slots for the leaderboard visual elements
    public GameObject leaderboardPanel;
    public TextMeshProUGUI leaderboardText;

    // Loads the main game scene
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Opens the leaderboard menu 
    public void OpenLeaderboard()
    {
        // Shows the leaderboard panel
        leaderboardPanel.SetActive(true);
        
        // Loads the secure leaderboard file
        LeaderboardData board = SecureSaveSystem.LoadLeaderboard();

        // Builds the text for the UI
        string text = "TOP SECURED SCORES\n\n";
        for (int i = 0; i < 10; i++)
        {
            if (board.scores[i] > 0)
            {
                text += (i + 1) + ". " + board.names[i] + " - " + board.scores[i] + "\n";
            }
        }
        
        leaderboardText.text = text;
    }

    // Closes the leaderboard menu
    public void CloseLeaderboard()
    {
        // Hides the leaderboard panel
        leaderboardPanel.SetActive(false);
    }
}