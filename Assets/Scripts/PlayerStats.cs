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

    public delegate void UpdateCurrentRoom(string roomName, int votesForRoom);
    public static event UpdateCurrentRoom OnCurrentRoomUpdate;

    public delegate void OnGameOver(bool success);
    public static event OnGameOver TriggerGameOver;

    public GameObject playerObject;

    public enum InventoryItems {
        key,
        inaccessible
    }

    public List<InventoryItems> currentInventory = new List<InventoryItems>();

    string channel_name = "";

    // stats + inventory reset by ResetPlayerStats
    float playerHealth;
    float playerMaxHealth;
    int playerCoins;
    string currentRoom;
    int votesForRoom;
    public bool gameOver = false;
    bool playerSucceeded = false;

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

            if (playerHealth == 0 && TriggerGameOver != null) TriggerGameOver.Invoke(false); // player failed
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

    public string CurrentRoom {
        set {
            currentRoom = value;
            votesForRoom = 0;

            if (OnCurrentRoomUpdate != null) OnCurrentRoomUpdate("test", 4);
        }
    }

    public void SetCurrentRoom(string newRoom, int numVotes) {
        currentRoom = newRoom;
        votesForRoom = numVotes;

        if (OnCurrentRoomUpdate != null) OnCurrentRoomUpdate(currentRoom, votesForRoom);
    }

    public bool GameWasSuccess {
        set {
            gameOver = true;
            playerSucceeded = value;
            if (TriggerGameOver != null) TriggerGameOver.Invoke(playerSucceeded);
        }
        get {
            return playerSucceeded;
        }
    }
}
