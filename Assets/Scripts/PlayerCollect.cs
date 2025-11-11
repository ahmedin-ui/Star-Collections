using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerCollect : MonoBehaviour
{
    public TextMeshProUGUI starText;
    private int totalStars;
    private int collectedStars = 0;

    public GameObject winPanel;
    public GameObject gameOverPanel;
    public int maxAttempts = 3;
    private int currentAttempts;
    public TextMeshProUGUI attemptText;
    public GameObject noAttemptsPanel;
    public GameObject mainMenuButton;

    void Start()
    {
        totalStars = GameObject.FindGameObjectsWithTag("Star").Length;
        starText.text = "Stars: " + collectedStars + " / " + totalStars;

        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);

       
        currentAttempts = PlayerPrefs.GetInt("AttemptsLeft", maxAttempts);
        UpdateAttemptUI();

        if (noAttemptsPanel != null)
        {
            noAttemptsPanel.SetActive(false);
        }
        if (mainMenuButton != null)
        {
            mainMenuButton.SetActive(false);
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered with: " + other.name + " (tag: " + other.tag + ")");

        if (other.CompareTag("Star"))
        {
            Destroy(other.gameObject);
            collectedStars++;
            starText.text = "Stars: " + collectedStars + " / " + totalStars;
            ScoreManager.Instance.AddStars(1);

            if (collectedStars >= totalStars)
            {
                WinGame();
            }
                
        }

        if (other.CompareTag("Obstacle"))
        {
            GameOver();
        }
            
    }

    void Update()
    {
        if (transform.position.y < -200)
        {
            GameOver();
        }
            
    }

    void WinGame()
    {
        winPanel.SetActive(true);
        Debug.Log("You Won!");
        int reward = 10;
        ScoreManager.Instance.AddStars(reward);
        Debug.Log("You won! +" + reward + " stars");

        
        PlayerPrefs.DeleteKey("AttemptsLeft");
    }

    void GameOver()
{
    // Show game over panel
    gameOverPanel.SetActive(true);
    Time.timeScale = 0f; // Pause the game

    // Decrease attempts
    currentAttempts--;

    if (currentAttempts > 0)
    {
        Debug.Log("Game Over! Attempts left: " + currentAttempts);
    }
    else
    {
        Debug.Log("All attempts are over!");

        
        if (noAttemptsPanel != null)
            {
                noAttemptsPanel.SetActive(true);
            }
            

        // Show Main Menu button (so player can exit)
        if (mainMenuButton != null)
            {
                mainMenuButton.SetActive(true);
            }
            
    }

    UpdateAttemptUI();
}


    public void RestartGame()
    {
        currentAttempts = PlayerPrefs.GetInt("AttemptsLeft", maxAttempts);

        if (currentAttempts > 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("No attempts left! Restart disabled.");
            if (noAttemptsPanel != null)
            {
                noAttemptsPanel.SetActive(true);
            }
                
        }
    }

    public void UpdateAttemptUI()
    {
        if (attemptText != null)
        {
             attemptText.text = "Attempts: " + currentAttempts + " / " + maxAttempts;
        }
           
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Ensure game is unpaused
        PlayerPrefs.DeleteKey("AttemptsLeft");
        SceneManager.LoadScene("MainMenuScene"); // Load Main Menu scene
    }
}