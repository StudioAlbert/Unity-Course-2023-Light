using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Coins") == false)
        {
            // No coins ------------------------
            PlayerPrefs.SetInt("Coins", 0);
        }
    }
    
}
