using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IObjectTriggered
{

    public enum PickupType {
        health,
        money
    }
    public PickupType pickupType = PickupType.health;

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
                // sound to indicate battery increase
                break;
            }
            case PickupType.money: {
                // the object is money! add to player stat
                Debug.Log("Money Picked up!");
                // sound to indicate money transaction
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
