using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IObjectTriggered))]
public class Interactable : MonoBehaviour
{

    public GameObject interactionKey;

    // which key should the user press?
    public char keyToPress = 'E';
    KeyCode inputKeyCode;

    public enum InteractionType {
        WhenNear,
        WhenKeyPressed
    }
    public InteractionType interactionType = InteractionType.WhenKeyPressed;

    bool showingInteractionKey = true;

    PlayerMovementController playerScript = null;
    public TMP_Text TMP_KeyToPress;

    public enum InteractionState {
        InRange,
        OutOfRange
    }
    InteractionState currentState = InteractionState.OutOfRange;

    IObjectTriggered scriptToTrigger;


    // Start is called before the first frame update
    void Start()
    {

        inputKeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), keyToPress.ToString());

        if (interactionKey == null) {
            interactionKey = GetComponentInChildren<GameObject>(); // this should get the first object
        }

        if (TMP_KeyToPress != null) {
            // make sure the key to press element has the right letter
            TMP_KeyToPress.text = keyToPress.ToString();
        }

        scriptToTrigger = GetComponent<IObjectTriggered>();
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

            // if we want to cause the interaction when they enter the space, execute
            if (interactionType == InteractionType.WhenNear) {
                Execute();
            }

            // in any case, the player is in range
            currentState = InteractionState.InRange;
        }
    }

    void OnTriggerExit2D(Collider2D collider2D) {

        // did the player just leave the space?
        if (collider2D.gameObject.GetComponent<PlayerMovementController>() != null) {
            currentState = InteractionState.OutOfRange;
            LeftRange();

            playerScript = null;
        }
    }

    void Execute() {

        // did we already trigger the event?
        if (!scriptToTrigger.triggered) {

            // no! so trigger it
            scriptToTrigger.TriggerObject();
        }
    }

    void LeftRange() {

        // did we end up triggering the event before?
        if (scriptToTrigger.triggered) {
            // yes! make sure the event knows the player left
            scriptToTrigger.LeftRange();
        }
    }
}
