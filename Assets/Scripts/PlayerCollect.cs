using UnityEngine;
using UnityEngine.UI;
using TMPro;  // 👈 Add this line

public class PlayerCollect : MonoBehaviour
{
    public TextMeshProUGUI starText; 
    public GameObject winPanel;

    private int totalStars;
    private int collectedStars = 0;
    public GameObject winPanel;
    public GameObject gameOverPanel;

    void Start()
    {
        totalStars = GameObject.FindGameObjectsWithTag("Star").Length;
        starText.text = "Stars: " + collectedStars + " / " + totalStars;
        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            Destroy(other.gameObject);
            collectedStars++;
            starText.text = "Stars: " + collectedStars + " / " + totalStars;

            if (collectedStars >= totalStars)
            {
                WinGame();
            }
        }

    }
    void Update()
    {
        // If player falls below Y = -5, trigger game over
        if (transform.position.y < -5)
        {
            GameOver();
        }
    }

    void WinGame()
    {
        winPanel.SetActive(true);
        Debug.Log("You Won!");
    }
    void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
}