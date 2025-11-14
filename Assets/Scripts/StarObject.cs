using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarObject : MonoBehaviour
{
    public string starID; // Assign unique ID in Inspector

    private void Start()
    {
        // If star was collected before, remove it
        if (PlayerPrefs.GetInt("Collected_" + starID, 0) == 1)
        {
            Destroy(gameObject);
        }
    }
}
