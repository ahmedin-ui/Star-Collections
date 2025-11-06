using UnityEngine;
using UnityEngine.UI;
using TMPro;  // 👈 Add this line

public class PlayerCollect : MonoBehaviour
{
    public TextMeshProUGUI starText; 
    public GameObject winPanel;

    private int totalStars;
    private int collectedStars = 0;

    void Start()
    {
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
    }
}