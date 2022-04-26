using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GearsUI : MonoBehaviour
{

    int gearCount = 0;
    public int GearCount {
        set {
            gearCount = value;

            if (gearsText != null) {
                gearsText.text = gearCount.ToString() + " b";
            }

        }
        get {
            return gearCount;
        }
    }

    TMP_Text gearsText;
    public PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        gearsText = GetComponent<TMP_Text>();
        PlayerStats.OnCoinUpdate += OnCoinUpdate;

        playerStats.PlayerCoins = playerStats.startingCoins;

    }

    void OnDisable() {
        PlayerStats.OnCoinUpdate -= OnCoinUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCoinUpdate(int newCoinVal) {
        GearCount = newCoinVal;
    }
}
