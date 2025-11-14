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

    [Header("UI Reference")]
    public TextMeshProUGUI StarCountText; // Assign this in Inspector

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadStars();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        TryFindStarText();
        UpdateStarUI();
        Debug.Log("Loaded Stars: " + Stars);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryFindStarText();
        UpdateStarUI();
    }

    private void TryFindStarText()
    {
        // If not assigned manually, try finding it automatically by name
        if (StarCountText == null)
        {
            var found = GameObject.Find("StarCountText");
            if (found != null)
            {
                StarCountText = found.GetComponent<TextMeshProUGUI>();
                Debug.Log("Found StarCountText in scene: " + found.name);
            }
        }
    }

    public void AddStars(int amount)
    {
        Stars += amount;
        SaveStars();
        UpdateStarUI();
    }

    public bool SpendStars(int amount)
    {
        if (Stars >= amount)
        {
            Stars -= amount;
            SaveStars();
            UpdateStarUI();
            Debug.Log($"Spent {amount} stars. Remaining: {Stars}");
            return true;
        }
        else
        {
            Debug.Log("Not enough stars to spend!");
            return false;
        }
    }

    private void UpdateStarUI()
    {
        if (StarCountText != null)
        {
            StarCountText.text = Stars.ToString();
        }
    }

    private void SaveStars()
    {
        PlayerPrefs.SetInt(STARS_KEY, Stars);
        PlayerPrefs.Save();
    }

    private void LoadStars()
    {
        Stars = PlayerPrefs.GetInt(STARS_KEY, 0);
    }

    // Optional cheat button
    public void CheatAddStars()
    {
        AddStars(50);
        Debug.Log("Cheat activated! +50 stars");
    }
    public int GetStars()
    {
        return Stars;
    }
}
