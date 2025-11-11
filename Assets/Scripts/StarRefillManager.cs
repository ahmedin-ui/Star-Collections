using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StarRefillManager : MonoBehaviour
{
    public TextMeshProUGUI starText;
    public TextMeshProUGUI timerText;

    private int stars;
    private const int refillAmount = 10;     // how many stars per refill
    private const int refillIntervalHours = 1; // refill every 1 hour
    private DateTime lastRefillTime;

    void Start()
    {
        // Load current stars
        stars = PlayerPrefs.GetInt("Stars", 0);

        // Load last refill time
        string savedTime = PlayerPrefs.GetString("LastRefillTime", "");
        if (!string.IsNullOrEmpty(savedTime))
        {
            lastRefillTime = DateTime.Parse(savedTime);
        }
        else
        {
            lastRefillTime = DateTime.Now;
            PlayerPrefs.SetString("LastRefillTime", lastRefillTime.ToString());
        }

        // Check for refills that happened while player was offline
        CheckForOfflineRefill();

        UpdateUI();
    }

    void Update()
    {
        TimeSpan timeSinceLastRefill = DateTime.Now - lastRefillTime;

        if (timeSinceLastRefill.TotalHours >= refillIntervalHours)
        {
            AddStars(refillAmount);
            lastRefillTime = DateTime.Now;
            PlayerPrefs.SetString("LastRefillTime", lastRefillTime.ToString());
            PlayerPrefs.Save();
        }

        // Update timer countdown
        TimeSpan remaining = TimeSpan.FromHours(refillIntervalHours) - timeSinceLastRefill;
        if (remaining.TotalSeconds < 0)
        {
            remaining = TimeSpan.Zero;

        timerText.text = $"Next refill in: {remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";
        }
        
    }

    void AddStars(int amount)
    {
        stars += amount;
        PlayerPrefs.SetInt("Stars", stars);
        PlayerPrefs.Save();
        UpdateUI();
        Debug.Log($"+{amount} stars added. Total = {stars}");
    }

    void CheckForOfflineRefill()
    {
        TimeSpan offlineTime = DateTime.Now - lastRefillTime;
        int hoursPassed = (int)offlineTime.TotalHours;

        if (hoursPassed > 0)
        {
            int starsToAdd = hoursPassed * refillAmount;
            AddStars(starsToAdd);
            lastRefillTime = DateTime.Now;
            PlayerPrefs.SetString("LastRefillTime", lastRefillTime.ToString());
            PlayerPrefs.Save();
        }
    }

    void UpdateUI()
    {
        starText.text = "Stars: " + stars;
    }
}