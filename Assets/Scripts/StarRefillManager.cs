using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StarRefillManager : MonoBehaviour
{
    public TextMeshProUGUI starText;
    public TextMeshProUGUI timerText;

    private int stars;
    private const int refillAmount = 50;
    private const int refillIntervalMinutes = 30; 
    private DateTime lastRefillTime;

    void Start()
    {
        // Load saved stars and last refill time
        stars = PlayerPrefs.GetInt("Stars", 0);

        string savedTime = PlayerPrefs.GetString("LastStarRefillTime", "");
        if (!string.IsNullOrEmpty(savedTime))
        {
            lastRefillTime = DateTime.Parse(savedTime);
        }
        else
        {
            lastRefillTime = DateTime.Now;
            PlayerPrefs.SetString("LastStarRefillTime", lastRefillTime.ToString());
        }

        // Check if player deserves offline refill
        CheckForOfflineRefill();
        UpdateUI();
    }

    void Update()
{
    // Calculate how long it's been since last refill
    TimeSpan timeSinceLastRefill = DateTime.Now - lastRefillTime;

    // Check if it's time to refill stars
    if (timeSinceLastRefill.TotalMinutes >= refillIntervalMinutes)
    {
        AddStars(refillAmount);
        lastRefillTime = DateTime.Now;
        PlayerPrefs.SetString("LastStarRefillTime", lastRefillTime.ToString());
        PlayerPrefs.Save();
    }

    // Calculate remaining time until next refill
    TimeSpan remaining = TimeSpan.FromMinutes(refillIntervalMinutes) - timeSinceLastRefill;
    if (remaining.TotalSeconds < 0)
        {
            remaining = TimeSpan.Zero;
        }
        

    // Always display "Earning stars in:"
    if (timerText != null)
        {
            timerText.text = $"Earning stars in: {remaining.Minutes:D2}:{remaining.Seconds:D2}";
        }
        
}

    void AddStars(int amount)
    {
        stars += amount;
        PlayerPrefs.SetInt("Stars", stars);
        PlayerPrefs.Save();
        UpdateUI();
        Debug.Log($"+{amount} stars added! Total stars = {stars}");
    }

    void CheckForOfflineRefill()
    {
        TimeSpan offlineTime = DateTime.Now - lastRefillTime;
        int intervalsPassed = (int)(offlineTime.TotalMinutes / refillIntervalMinutes);

        if (intervalsPassed > 0)
        {
            int starsToAdd = intervalsPassed * refillAmount;
            AddStars(starsToAdd);
            lastRefillTime = DateTime.Now;
            PlayerPrefs.SetString("LastStarRefillTime", lastRefillTime.ToString());
            PlayerPrefs.Save();
        }
    }

    void UpdateUI()
    {
        starText.text = "Stars: " + stars;
    }
}