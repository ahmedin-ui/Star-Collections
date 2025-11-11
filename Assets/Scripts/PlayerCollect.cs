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
        currentAttempts = maxAttempts;
        UpdateAttemptUI();
        if (noAttemptsPanel != null)
        {
            noAttemptsPanel.SetActive(false);
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
        // If player falls below Y = -5, trigger game over
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
        ScoreManager.Instance.AddStars(reward); // Pause the game
        Debug.Log("You won! +" + reward + " stars");
    }
    void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        currentAttempts--;
        if (currentAttempts > 0)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("Game Over! Attempts left: " + currentAttempts);
        }
        else
        {
            if (noAttemptsPanel != null)
            {
                noAttemptsPanel.SetActive(true);
            }
            else
            {
                Debug.Log("No attempts left!");
            }
        }
        UpdateAttemptUI();
        Time.timeScale = 0f; // Pause the game
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; // resume game speed
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (currentAttempts > 0)
        {
            Time.timeScale = 1f; // resume game speed
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("No attempts left! Retry disabled.");
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

}