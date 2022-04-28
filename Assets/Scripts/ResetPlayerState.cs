using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerState : MonoBehaviour
{
    public float startingHealth = 0.5f;
    public float startingMaxHealth = 3f;
    public int startingCoins = 0;
    public PlayerStats playerStats;

    // Start is called before the first frame update
    void Awake()
    {
        playerStats.PlayerMaxHealth = startingMaxHealth;
        playerStats.PlayerHealth = startingHealth;
        playerStats.PlayerCoins = startingCoins;

        playerStats.currentInventory = new List<PlayerStats.InventoryItems>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
