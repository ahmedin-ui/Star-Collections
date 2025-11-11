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

    void Start()
    {
        totalStars = GameObject.FindGameObjectsWithTag("Star").Length;
        starText.text = "Stars: " + collectedStars + " / " + totalStars;

        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // ✅ Load attempts from PlayerPrefs, or start at max if none saved yet
        currentAttempts = PlayerPrefs.GetInt("AttemptsLeft", maxAttempts);
        UpdateAttemptUI();

        if (noAttemptsPanel != null)
            noAttemptsPanel.SetActive(false);
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
                WinGame();
        }

        if (other.CompareTag("Obstacle"))
            GameOver();
    }

    void Update()
    {
        if (transform.position.y < -200)
            GameOver();
    }

    void WinGame()
    {
        winPanel.SetActive(true);
        Debug.Log("You Won!");
        int reward = 10;
        ScoreManager.Instance.AddStars(reward);
        Debug.Log("You won! +" + reward + " stars");

        // ✅ Reset attempts after winning
        PlayerPrefs.DeleteKey("AttemptsLeft");
    }

    void GameOver()
    {
        Time.timeScale = 0f;

        if (currentAttempts > 1)
        {
            currentAttempts--;
            PlayerPrefs.SetInt("AttemptsLeft", currentAttempts);
            PlayerPrefs.Save();

            gameOverPanel.SetActive(true);
            Debug.Log("Game Over! Attempts left: " + currentAttempts);
        }
        else
        {
            currentAttempts = 0;
            PlayerPrefs.SetInt("AttemptsLeft", currentAttempts);
            PlayerPrefs.Save();

            if (noAttemptsPanel != null)
                noAttemptsPanel.SetActive(true);
            else
                Debug.Log("No attempts left!");
        }

        UpdateAttemptUI();
    }

    public void RestartGame()
    {
        int savedAttempts = PlayerPrefs.GetInt("AttemptsLeft", maxAttempts);

        if (savedAttempts > 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("No attempts left! Restart disabled.");
            if (noAttemptsPanel != null)
                noAttemptsPanel.SetActive(true);
        }
    }

    public void UpdateAttemptUI()
    {
        if (attemptText != null)
            attemptText.text = "Attempts: " + currentAttempts + " / " + maxAttempts;
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Ensure game is unpaused
        SceneManager.LoadScene("MainMenu"); // Load Main Menu scene
    }
}