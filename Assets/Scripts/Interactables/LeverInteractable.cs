using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverInteractable : MonoBehaviour, IObjectTriggered
{

    // trigger once until puzzle is reset
    bool _triggered = false;

    public int lever_number = 0; // zero indexed 0.0

    public bool triggered {
        set {
            _triggered = value;

            // theoretically flip the levers
            if (!_triggered) {
                // the normal state
                spriteRenderer.color = new Color(1, 100f/255f, 1);
            } else {
                // triggered state
                spriteRenderer.color = new Color(130f/255f, 65f/255f, 130f/255f);
            }
        } 
        get {
            return _triggered;
        }
    }

    PuzzleManager puzzleManager;
    SpriteRenderer spriteRenderer;
    DialogueBox dialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        puzzleManager = GetComponentInParent<PuzzleManager>();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1]; // assuming the thing itself is after the interaction key sprite
        dialogueBox = GetComponent<DialogueBox>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerObject() {
        // flip the lever!
        triggered = true;
        puzzleManager.TurnOnLever(lever_number);

        Debug.Log("Turn lever "+lever_number.ToString()+" on!");
    }

    public void LeftRange() {

    }

    public void ResetLever() {
        triggered = false;
        dialogueBox.ResetDialogueLoop();
    }
}
