using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IObjectTriggered))]
public class Interactable : MonoBehaviour
{

    public GameObject interactionKey;

    // which key should the user press?
    public string keyToPress = "Space";
    KeyCode inputKeyCode;

    public enum InteractionType {
        WhenNear,
        WhenKeyPressed
    }
    public InteractionType interactionType = InteractionType.WhenKeyPressed;

    bool showingInteractionKey = true;

    public PlayerStats.InventoryItems[] requiredToInteract;

    PlayerMovementController playerScript = null;
    public TMP_Text TMP_KeyToPress;
    public PlayerStats playerStats;

    public enum InteractionState {
        InRange,
        OutOfRange
    }
    InteractionState currentState = InteractionState.OutOfRange;

    IObjectTriggered[] scriptsToTrigger;


    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindObjectOfType<ChatManager>().GetComponent<ChatManager>().playerStats;

        inputKeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), keyToPress);

        if (interactionKey == null) {
            interactionKey = GetComponentInChildren<GameObject>(); // this should get the first object
        }

        if (TMP_KeyToPress != null) {
            // make sure the key to press element has the right string
            if (keyToPress.ToLower().Equals("space")) {
                // if we want them to press space, make it lower case
                TMP_KeyToPress.text = keyToPress.ToLower();
            } else {
                // otherwise, just put the string in
                TMP_KeyToPress.text = keyToPress.ToString();
            }
        }

        scriptsToTrigger = GetComponents<IObjectTriggered>();
    }

    // Update is called once per frame
    void Update()
    {

        // are we in range but not showing interaction key?
        if (currentState==InteractionState.OutOfRange && showingInteractionKey) {
            // turn off the key icon
            interactionKey.gameObject.SetActive(false);
            showingInteractionKey = false;

        // are we out of range but showing the interaction key?
        } else if (currentState==InteractionState.InRange && !showingInteractionKey) {
            // turn on the key icon
            interactionKey.gameObject.SetActive(true);
            showingInteractionKey = true;
        }

        if (currentState==InteractionState.InRange && Input.GetKeyDown(inputKeyCode)) {
            // run the object interaction if the player is in range and they pressed the button
            Execute();
        }
        
    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        playerScript = collider2D.gameObject.GetComponent<PlayerMovementController>();

        // did the Player enter the space?
        if (playerScript != null) {
            bool missingItem = false;

            foreach (PlayerStats.InventoryItems item in requiredToInteract) {
                Debug.Log("item is equal to key: "+(item==PlayerStats.InventoryItems.key).ToString());

                if (!playerStats.currentInventory.Contains(item)) {
                    missingItem = true;
                    Debug.Log("Missing item: "+item.ToString());
                }
            }

            if (!missingItem) {
                // if we want to cause the interaction when they enter the space, execute
                if (interactionType == InteractionType.WhenNear) {
                    Execute();
                }

                // in any case, the player is in range
                currentState = InteractionState.InRange;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider2D) {

        // did the player just leave the space?
        if (collider2D.gameObject.GetComponent<PlayerMovementController>() != null) {
            LeftRange();

            playerScript = null;
        }
    }

    void Execute() {

        // did we already trigger the event?
        foreach (IObjectTriggered sTrigger in scriptsToTrigger) {
            if (!sTrigger.triggered) {
                // no! so trigger it
                sTrigger.TriggerObject();
            }
        }
        
    }

    public void LeftRange() {
        currentState = InteractionState.OutOfRange;
        foreach (IObjectTriggered sTrigger in scriptsToTrigger) {
            // did we end up triggering the event before?
            if (sTrigger.triggered || sTrigger.GetType() == typeof(GoodGameOverInteractable)) {
                // yes! make sure the event knows the player left
                sTrigger.LeftRange();
            }
        }
    }
}
