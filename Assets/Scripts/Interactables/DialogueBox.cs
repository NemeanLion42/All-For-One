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
            // yes! destroy the dialogue box object
            Destroy(dialogueInstance);
            triggered = false;
            dialogueInstance = null;
        }
    }

    public void WriteText(string text) {
        // make it easier to change the text
        if (dialogueText != null) {
            dialogueText.text = text;
        } 
    } 
    public void TriggerObject() {
        triggered = true;

        // Create a dialogue box instance and set it's parent to the canvas
        dialogueInstance = Instantiate(dialoguePrefab, Vector3.zero, Quaternion.identity);
        dialogueInstance.transform.SetParent(uiCanvas.transform);
        // we need to make sure the dialogueBoxRectangle is just standard in the local space
        RectTransform dialogueBoxRect = dialogueInstance.GetComponent<RectTransform>();
        dialogueBoxRect.localPosition = Vector3.zero;
        dialogueBoxRect.localScale = new Vector3(1, 1, 1);
        
        // update out text and background objects
        dialogueText = dialogueInstance.GetComponentInChildren<TMP_Text>();
        dialogueBackground = dialogueInstance.GetComponentInChildren<Image>();

        // make sure the dialogue box says the dialogue
        WriteText(textToWrite[text_idx]);

        // are we at the end of the list?
        if (text_idx == textToWrite.Length - 1) {
            // yes! loop the text if the bool is active, otherwise let it be
            if (loopText) {text_idx = 0;}
        
        } else {
            // no, increment through
            text_idx++;
        }
    }

    public void LeftRange() {
        // the player left the range so we should delete the dialogue box
        triggered = false;

        if (dialogueInstance != null) {
            Destroy(dialogueInstance);
            dialogueInstance = null;
        }
    }
}
