using UnityEngine;
using UnityEngine.UI;  // for UI
using UnityEngine.SceneManagement; // optional if you want to reload scenes

public class PlayerCollect : MonoBehaviour
{
    public Text starText;           // Assign from UI
    public GameObject winPanel;     // Assign from UI

    private int totalStars;
    private int collectedStars = 0;

    void Start()
    {
        // Count all stars in the scene
        totalStars = GameObject.FindGameObjectsWithTag("Star").Length;
        starText.text = "Stars: " + collectedStars + " / " + totalStars;
        winPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            Destroy(other.gameObject);
            collectedStars++;
            starText.text = "Stars: " + collectedStars + " / " + totalStars;

            // Check if all stars collected
            if (collectedStars >= totalStars)
            {
                WinGame();
            }
        }
    }

    void WinGame()
    {
        winPanel.SetActive(true);
        Debug.Log("You Won!");
        // Optionally stop movement or show animation here
    }
}
