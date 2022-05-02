using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour, IObjectTriggered
{
    public string[] textToWrite = new string[] {
        "test text",
        "oof"
    };
    public bool loopText = true;
    public bool dontFlip = false;
    bool justTriggered = false;

    bool _triggered = false;
    public bool triggered {
        set {
            _triggered = value;
        }
        get {
            return _triggered;
        }
    }

    int text_idx = 0;

    Canvas uiCanvas;
    public GameObject dialoguePrefab;
    TMP_Text dialogueText;
    Image dialogueBackground;
    GameObject dialogueInstance;

    void Start()
    {
        uiCanvas = FindObjectOfType<Canvas>(); // there should only be one Canvas in the Scene
    }

    // Update is called once per frame
    void Update()
    {
        // is the dialogue box showing and did the user press space?
        if (triggered && Input.GetKeyDown(KeyCode.Space)) {
            if (justTriggered) {
                justTriggered = false;
                return;
            }
            // yes! destroy the dialogue box object
            // Destroy(dialogueInstance);
            // triggered = false;
            // dialogueInstance = null;


            // should we flip the page on interaction?
            if (dontFlip) {
                // no, just remove the textbox and increment the index
                Destroy(dialogueInstance);
                triggered = false;
                dialogueInstance = null;

                // make sure to reset the text or back off out of index if we're not flipping and on the last page
                if (text_idx == textToWrite.Length) {
                    if (loopText) text_idx = 0;
                    else text_idx--;
                }
            } else {
                // yep! flip the page
                FlipPage();
            }

        }
    }

    public void ResetDialogueLoop() {
        text_idx = 0;
    }

    public void WriteText(string text) {
        // make it easier to change the text
        if (dialogueText != null) {
            dialogueText.text = text;
        } 
    } 
    public void TriggerObject() {
        triggered = true;
        justTriggered = true;
        // if we want an audio clip that sounds like typing, it would likely trigger here

        // Create a dialogue box instance and set it's parent to the canvas
        dialogueInstance = Instantiate(dialoguePrefab, Vector3.zero, Quaternion.identity);
        dialogueInstance.transform.SetParent(uiCanvas.transform.GetChild(0).transform);
        // we need to make sure the dialogueBoxRectangle is just standard in the local space
        RectTransform dialogueBoxRect = dialogueInstance.GetComponent<RectTransform>();
        dialogueBoxRect.localPosition = new Vector3(0, 15, 0);
        dialogueBoxRect.localScale = new Vector3(1, 1, 1);
        
        // update out text and background objects
        dialogueText = dialogueInstance.GetComponentInChildren<TMP_Text>();
        dialogueBackground = dialogueInstance.GetComponentInChildren<Image>();

        // text_idx -= 1; // space triggers both

        // make sure the dialogue box says the dialogue
        FlipPage();

        // are we at the end of the list?
        // if (text_idx == textToWrite.Length - 1) {
        //     // yes! loop the text if the bool is active, otherwise let it be
        //     // cut off typing audio/loop here?
        //     // if (loopText) {text_idx = 0;}
        
        // } else {
        //     // no, increment through
        //     text_idx++;
        // }
    }

    public void LeftRange() {
        // the player left the range so we should delete the dialogue box
        triggered = false;
        justTriggered = false;

        if (dialogueInstance != null) {
            Destroy(dialogueInstance);
            dialogueInstance = null;

            // if the player just leaves, we should make sure that we're not out of bounds
            if (text_idx == textToWrite.Length) {
                    if (loopText) text_idx = 0;
                    else text_idx--;
            }

        }
    }

    public void FlipPage() {
        if (text_idx == textToWrite.Length) {
            Destroy(dialogueInstance);
            triggered = false;
            justTriggered = false;
            dialogueInstance = null;

            if (loopText) text_idx = 0;
            else text_idx --;

            return;
        }

        if (text_idx != -1) WriteText(textToWrite[text_idx]);
        text_idx++;
    }
}
