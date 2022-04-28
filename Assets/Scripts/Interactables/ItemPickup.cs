using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IObjectTriggered
{

    public enum PickupType {
        health,
        halfHealth,
        money,
        key
    }
    public PickupType pickupType = PickupType.health;
    public PlayerStats playerStats;

    public bool triggered {
        set {

        }
        get {
            return false;
        }
    }

    public void TriggerObject() {
        // do the thing

        switch (pickupType) {
            case PickupType.health: {
                // the object is health! add to player stat
                Debug.Log("Health picked up!");
                playerStats.PlayerHealth += 1.0f;
                break;
            }
            case PickupType.halfHealth: {
                playerStats.PlayerHealth += 0.5f;
                break;
            }
            case PickupType.money: {
                // the object is money! add to player stat
                playerStats.PlayerCoins += 5;
                break;
            }
            case PickupType.key: {
                // the object is a key! add it to the player inventory
                playerStats.currentInventory.Add(PlayerStats.InventoryItems.key);

                foreach (PlayerStats.InventoryItems i in playerStats.currentInventory) {
                    Debug.Log("Item in inventory: "+i.ToString());
                }
                break;
            }
            default: {
                break;
            }
        }

        // item was picked up so we no longer need it in the world
        Destroy(gameObject);
    } 

    public void LeftRange() {
        // probably unused for object pickup
    }
}
