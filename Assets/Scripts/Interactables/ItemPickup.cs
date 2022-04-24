using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IObjectTriggered
{

    public enum PickupType {
        health,
        halfHealth,
        money
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
                Debug.Log("Money Picked up!");
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
