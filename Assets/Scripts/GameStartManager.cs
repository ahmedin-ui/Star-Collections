using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
{
    Debug.Log("StartGame() button pressed");
    int entryStars = 10;

    if (ScoreManager.Instance == null)
    {
        Debug.LogError("ScoreManager instance not found! Make sure it's in the MainMenu scene.");
        return;
    }

    if (ScoreManager.Instance.SpendStars(entryStars))
    {
        Debug.Log("10 stars spent! Loading GameScene...");
        SceneManager.LoadScene("GameScene");
    }
    else
    {
        Debug.Log("Not enough stars to play!");
    }
}
}
