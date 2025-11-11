using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        int entryStars = 5; // ⭐ Entry cost

        if (EconomyManager.Instance.SpendStars(entryStars))
        {
            Debug.Log("5 Stars spent! Starting game...");
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("Not enough stars to play!");
        }
    }
}
