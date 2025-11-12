using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

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

    public TextMeshProUGUI countdownText; 

    private bool isGameOver = false;

    void Start()
    {
        totalStars = GameObject.FindGameObjectsWithTag("Star").Length;
        starText.text = "Stars: " + collectedStars + " / " + totalStars;

        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        currentAttempts = PlayerPrefs.GetInt("AttemptsLeft", maxAttempts);
        UpdateAttemptUI();

        if (noAttemptsPanel != null)
            noAttemptsPanel.SetActive(false);
        if (mainMenuButton != null)
            mainMenuButton.SetActive(false);
        if (countdownText != null)
            countdownText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
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
        int reward = 10;
        ScoreManager.Instance.AddStars(reward);
        PlayerPrefs.DeleteKey("AttemptsLeft");
    }

    void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        currentAttempts--;
        PlayerPrefs.SetInt("AttemptsLeft", currentAttempts);
        PlayerPrefs.Save();

        if (currentAttempts > 0)
        {
            Debug.Log("Game Over! Attempts left: " + currentAttempts);
        }
        else
        {
            Debug.Log("All attempts are over!");

            if (noAttemptsPanel != null)
                noAttemptsPanel.SetActive(true);

            if (mainMenuButton != null)
                mainMenuButton.SetActive(true);

            // Start countdown
            StartCoroutine(ResetAttemptsAfterDelay(30f));
        }

        UpdateAttemptUI();
    }

    IEnumerator ResetAttemptsAfterDelay(float delay)
    {
        float remaining = delay;

        while (remaining > 0)
        {
            if (countdownText != null)
                countdownText.text = "Restarting in: " + Mathf.CeilToInt(remaining) + "s";

            yield return new WaitForSecondsRealtime(1f);
            remaining -= 1f;
        }

        // Reset attempts
        currentAttempts = maxAttempts;
        PlayerPrefs.SetInt("AttemptsLeft", currentAttempts);
        PlayerPrefs.Save();

        if (countdownText != null)
            countdownText.text = "Restarting...";

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartGame()
    {
        if (currentAttempts > 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
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
        Time.timeScale = 1f;
        PlayerPrefs.DeleteKey("AttemptsLeft");
        SceneManager.LoadScene("MainMenuScene");
    }
}