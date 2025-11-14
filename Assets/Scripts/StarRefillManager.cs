using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarRefillManager : MonoBehaviour
{
    public TextMeshProUGUI starText;
    public TextMeshProUGUI timerText;

    private int stars;
    private const int refillAmount = 20;
    private const float refillInterval = 30f;
    private float refillTimer = 0f;

    void Start()
    {
        stars = ScoreManager.Instance.GetStars();
        refillTimer = 0f;
        UpdateUI();
    }

    void Update()
    {
        refillTimer += Time.deltaTime;

        if (refillTimer >= refillInterval)
        {
            AddStars(refillAmount);
            refillTimer = 0f;
        }

        UpdateTimerUI();
    }

    void AddStars(int amount)
    {
        ScoreManager.Instance.AddStars(amount);
        stars = ScoreManager.Instance.GetStars();
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