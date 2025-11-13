using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DailyLoginManager : MonoBehaviour
{
    public TextMeshProUGUI rewardText;   // Text to show daily reward message
    public TextMeshProUGUI totalStarsText; // Optional — shows total stars

    private int totalStars;
    private int currentDay;
    private DateTime lastLoginDate;

    void Start()
    {
        // Load saved data
        totalStars = PlayerPrefs.GetInt("Stars", 0);
        currentDay = PlayerPrefs.GetInt("LoginDay", 0);

        string savedDate = PlayerPrefs.GetString("LastLoginDate", "");
        if (!string.IsNullOrEmpty(savedDate))
        {
            lastLoginDate = DateTime.Parse(savedDate);
        }

        else
        {
            lastLoginDate = DateTime.MinValue;
        }
            

        CheckDailyLogin();
        UpdateUI();
    }

    void CheckDailyLogin()
    {
        DateTime today = DateTime.Now.Date;

        // First login ever
        if (lastLoginDate == DateTime.MinValue)
        {
            GiveReward();
            return;
        }

        // Already claimed today
        if (lastLoginDate.Date == today)
        {
            rewardText.text = "Already claimed today!";
            return;
        }

        // If it’s a new day → give reward
        if ((today - lastLoginDate.Date).TotalDays >= 1)
        {
            GiveReward();
        }
    }

    void GiveReward()
    {
        currentDay++;

        // Reward logic: 10, 20, 30 (from day 3 onwards, always 30)
        int reward = currentDay == 1 ? 10 :
                     currentDay == 2 ? 20 : 30;

        totalStars += reward;

        // Save progress
        PlayerPrefs.SetInt("Stars", totalStars);
        PlayerPrefs.SetInt("LoginDay", currentDay);
        PlayerPrefs.SetString("LastLoginDate", DateTime.Now.ToString());
        PlayerPrefs.Save();

        rewardText.text = $"Daily Reward: +{reward} stars (Day {currentDay})";
        Debug.Log($"Daily reward: {reward} stars | Day {currentDay}");
    }

    void UpdateUI()
    {
        if (totalStarsText != null)
        {
            totalStarsText.text = $"Stars: {totalStars}";
        }
            
    }
}
