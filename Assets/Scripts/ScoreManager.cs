using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private const string STARS_KEY = "PlayerStars";
    public int Stars { get; private set; }

    [Header("UI Reference (Optional)")]
    public TextMeshProUGUI StarText; // Drag your LobbyScene text here (optional)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadStars();
    }

    private void Start()
    {
        UpdateStarUI();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to find the "StarText_Game" object automatically in new scenes
        if (StarText == null)
        {
            var foundText = GameObject.Find("StarText_Game");
            if (foundText != null)
            {
                StarText = foundText.GetComponent<TextMeshProUGUI>();
                UpdateStarUI();
            }
        }
    }

    public void AddStars(int amount)
    {
        Stars += amount;
        SaveStars();
        UpdateStarUI();
    }

    public void SaveStars()
    {
        PlayerPrefs.SetInt(STARS_KEY, Stars);
        PlayerPrefs.Save();
    }

    public void LoadStars()
    {
        Stars = PlayerPrefs.GetInt(STARS_KEY, 0);
    }

    private void UpdateStarUI()
    {
        if (StarText != null)
            StarText.text = "Stars: " + Stars.ToString();
    }
}