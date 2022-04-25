using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerStatsScriptableObj", order = 1)]
public class PlayerStats : ScriptableObject
{

    public delegate void UpdateCurrentHealth(float newHealth);
    public static event UpdateCurrentHealth OnCurrentHealthUpdate;
    public delegate void UpdateMaxHealth(float newMaxHealth);
    public static event UpdateMaxHealth OnMaxHealthUpdate;

    public delegate void UpdateCoins(int newCoins);
    public static event UpdateCoins OnCoinUpdate;

    string channel_name = "";

    float playerHealth = 1f;
    float playerMaxHealth = 4f;


    public int startingCoins = 0;
    int playerCoins = 0;

    public string ChannelName {
        get {
            return channel_name;
        } 
        set {
            // we want to make sure that channel names are lower case and trimmed of leading/lagging whitespace
            channel_name = value.ToLower().Trim(); 
        }
    }

    public float PlayerHealth {
        set {
            playerHealth = Mathf.Clamp(value, 0.0f, playerMaxHealth);
            if (OnCurrentHealthUpdate != null) OnCurrentHealthUpdate.Invoke(playerHealth);
        }
        get {
            return playerHealth;
        }
    }

    public float PlayerMaxHealth {
        set {
            playerMaxHealth = Mathf.Max(1.0f, value);

            if (OnMaxHealthUpdate != null) OnMaxHealthUpdate.Invoke(playerMaxHealth);
        }
        get {
            return playerMaxHealth;
        }
    }

    public int PlayerCoins {
        set {
            playerCoins = value;

            if (OnCoinUpdate != null) OnCoinUpdate.Invoke(playerCoins);
        }
        get {
            return playerCoins;
        }
    }
}
