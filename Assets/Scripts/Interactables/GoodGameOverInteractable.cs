using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodGameOverInteractable : MonoBehaviour, IObjectTriggered
{

    bool _triggered = false;
    int triggeredCount = 0;
    public int triggersUntilGameOver = 3; 
    public PlayerStats playerStats;

    public bool triggered {
        set {
            // _triggered = value;
            if (value) triggeredCount++;
        } 
        get {
            return _triggered;
        }
    }


    DialogueBox dialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox = GetComponent<DialogueBox>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerObject() {
        triggered = true;

        if (triggeredCount >= triggersUntilGameOver) {
            // trigger the game over state somehow
            dialogueBox.LeftRange();
            playerStats.GameWasSuccess = true;
        }
    }

    public void LeftRange() {
        // only increment the triggeredCounter if the player has interacted with it at least once
        if (triggeredCount >= 1) triggeredCount++;
    }
}
