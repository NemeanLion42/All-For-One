using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IObjectTriggered))]
public class Interactable : MonoBehaviour
{

    public GameObject interactionKey;
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
            TMP_KeyToPress.text = keyToPress.ToString();
        }

        scriptToTrigger = GetComponent<IObjectTriggered>();
    }

    void Awake() {
    }

    // Update is called once per frame
    void Update()
    {

        // are we in range but not showing interaction key?
        if (currentState==InteractionState.OutOfRange && showingInteractionKey) {
            interactionKey.gameObject.SetActive(false);
            showingInteractionKey = false;

        // are we out of range but showing the interaction key?
        } else if (currentState==InteractionState.InRange && !showingInteractionKey) {
            interactionKey.gameObject.SetActive(true);
            showingInteractionKey = true;
        }

        if (currentState==InteractionState.InRange && Input.GetKeyDown(inputKeyCode)) {
            Execute();
        }
        
    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        playerScript = collider2D.gameObject.GetComponent<PlayerMovementController>();

        if (playerScript != null) {

            if (interactionType == InteractionType.WhenNear) {
                Execute();
            }

            currentState = InteractionState.InRange;
        }
    }

    void OnTriggerExit2D(Collider2D collider2D) {

        if (collider2D.gameObject.GetComponent<PlayerMovementController>() != null) {
            currentState = InteractionState.OutOfRange;
            LeftRange();

            playerScript = null;
        }
    }

    void Execute() {
        if (!scriptToTrigger.triggered) {
            scriptToTrigger.TriggerObject();
        }
    }

    void LeftRange() {
        if (scriptToTrigger.triggered) {
            scriptToTrigger.LeftRange();
        }
    }
}
