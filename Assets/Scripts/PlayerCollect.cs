using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerCollect : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI starText;
    public TextMeshProUGUI attemptText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI totalStarsText;

    [Header("Panels")]
    public GameObject winPanel;
    public GameObject gameOverPanel;
    public GameObject noAttemptsPanel;

    [Header("Game Settings")]
    public int maxAttempts = 3;

    private int totalStars;
    private int collectedStars = 0;
    private int currentAttempts;
    private bool isGameOver = false;
    [Header("Hearts UI")]
    public Image heart1;
    public Image heart2;
    public Image heart3;

    void Start()
    {
        // Initialize total stars in the level
        totalStars = GameObject.FindGameObjectsWithTag("Star").Length;
        if (starText != null)
            starText.text = $"Stars: {collectedStars} / {totalStars}";

        // Panels
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (noAttemptsPanel != null) noAttemptsPanel.SetActive(false);

        // Attempts
        currentAttempts = PlayerPrefs.GetInt("AttemptsLeft", maxAttempts);
        if (currentAttempts <= 0)
        {
            currentAttempts = maxAttempts;
            PlayerPrefs.SetInt("AttemptsLeft", currentAttempts);
            PlayerPrefs.Save();
        }
        UpdateAttemptUI();

        // Countdown
        if (countdownText != null) countdownText.text = "";

        // Total stars display
        if (totalStarsText != null && ScoreManager.Instance != null)
        {
            totalStarsText.text = "Total: " + ScoreManager.Instance.GetStars();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (other.CompareTag("Star"))
        {
            Destroy(other.gameObject);
            collectedStars++;
            if (starText != null)
                starText.text = $"Stars: {collectedStars} / {totalStars}";

            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddStars(1);

            if (collectedStars >= totalStars)
                WinGame();
        }

        if (other.CompareTag("Obstacle"))
        {
            GameOver();
        }

        if (totalStarsText != null && ScoreManager.Instance != null)
            totalStarsText.text = "Total: " + ScoreManager.Instance.GetStars();
    }

    void Update()
    {
        // Fall detection
        if (!isGameOver && transform.position.y < -200)
        {
            GameOver();
        }
    }

    void WinGame()
    {
        if (winPanel != null) winPanel.SetActive(true);

        if (ScoreManager.Instance != null)
        {
            int reward = 20; // Reward for winning
            ScoreManager.Instance.AddStars(reward);
        }

        PlayerPrefs.DeleteKey("AttemptsLeft");

        if (totalStarsText != null && ScoreManager.Instance != null)
            totalStarsText.text = "Total: " + ScoreManager.Instance.GetStars();
    }

    void GameOver()
{
    if (isGameOver) return;
    isGameOver = true;

    if (gameOverPanel != null)
        gameOverPanel.SetActive(true);

    Time.timeScale = 0f;

    // Only decrement once and clamp to 0
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

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddStars(-10);

        StartCoroutine(ResetAttemptsAfterDelay(10f));
    }
    
    UpdateAttemptUI();
}

    IEnumerator ResetAttemptsAfterDelay(float delay)
    {
        float remaining = delay;

        while (remaining > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = "Restarting in: " + Mathf.CeilToInt(remaining) + "s";
            }

            yield return new WaitForSecondsRealtime(1f);
            remaining -= 1f;
        }

        // Reset attempts
        currentAttempts = maxAttempts;
        PlayerPrefs.SetInt("AttemptsLeft", currentAttempts);
        PlayerPrefs.Save();

        if (countdownText != null)
        {
            countdownText.text = "Restarting...";
            Time.timeScale = 1f;
        }

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

    void UpdateAttemptUI()
    {
        AnimateHeart(heart1, currentAttempts >= 1);
        AnimateHeart(heart2, currentAttempts >= 2);
        AnimateHeart(heart3, currentAttempts >= 3);
    }

    void AnimateHeart(Image heart, bool show)
    {
        if (heart == null) return;

        if (show)
        {
            heart.gameObject.SetActive(true);
            heart.rectTransform.localScale = Vector3.one;
        }
        else
        {
            StartCoroutine(FallHeart(heart));
        }
    }

    IEnumerator FallHeart(Image heart)
    {
        Vector3 originalPos = heart.rectTransform.localPosition;
        Vector3 targetPos = originalPos + new Vector3(0, -50f, 0);

        float timer = 0f;
        float duration = 0.3f;

        while (timer < duration)
        {
            heart.rectTransform.localPosition = Vector3.Lerp(originalPos, targetPos, timer / duration);
            heart.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / duration);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        heart.rectTransform.localPosition = originalPos;
        heart.rectTransform.localScale = Vector3.one;
        heart.gameObject.SetActive(false);
    }
}