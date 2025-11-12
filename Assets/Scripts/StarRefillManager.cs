using UnityEngine;
using TMPro;

public class StarRefillManager : MonoBehaviour
{
    public TextMeshProUGUI starText;
    public TextMeshProUGUI timerText;

    private int stars;
    private const int refillAmount = 50;
    private const float refillInterval = 1800f; // 30 minutes = 1800 seconds
    private float refillTimer = 0f;

    void Start()
    {
        // Load saved stars only (no real time)
        stars = PlayerPrefs.GetInt("Stars", 0);
        refillTimer = 0f;
        UpdateUI();
    }

    void Update()
    {
        refillTimer += Time.deltaTime; // counts only while game is open

        if (refillTimer >= refillInterval)
        {
            AddStars(refillAmount);
            refillTimer = 0f; // reset timer
        }

        UpdateTimerUI();
    }

    void AddStars(int amount)
    {
        stars += amount;
        PlayerPrefs.SetInt("Stars", stars);
        PlayerPrefs.Save();
        UpdateUI();
        Debug.Log($"+{amount} stars added! Total stars = {stars}");
    }

    void UpdateUI()
    {
        if (starText != null)
            starText.text = "Stars: " + stars;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            float remaining = Mathf.Max(refillInterval - refillTimer, 0f);
            int minutes = Mathf.FloorToInt(remaining / 60);
            int seconds = Mathf.FloorToInt(remaining % 60);
            timerText.text = $"Earning stars in: {minutes:00}:{seconds:00}";
        }
    }
}