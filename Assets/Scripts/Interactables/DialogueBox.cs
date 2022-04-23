using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBox : MonoBehaviour, IObjectTriggered
{
    public string textToWrite = "test";

    bool _triggered = false;
    public bool triggered {
        set {
            _triggered = value;
        }
        get {
            return _triggered;
        }
    }

    Canvas uiCanvas;
    public GameObject dialoguePrefab;
    TMP_Text dialogueText;
    Image dialogueBackground;

    GameObject dialogueInstance;

    // Start is called before the first frame update
    void Start()
    {
        uiCanvas = FindObjectOfType<Canvas>(); // there should only be one Canvas in the Scene
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered && Input.GetKeyDown(KeyCode.Space)) {
            Destroy(dialogueInstance);
            triggered = false;
            dialogueInstance = null;
        }
    }

    public void WriteText(string text) {
        if (dialogueText != null) {
            dialogueText.text = text;
        } 
    } 
    public void TriggerObject() {
        triggered = true;

        dialogueInstance = Instantiate(dialoguePrefab, Vector3.zero, Quaternion.identity);
        dialogueInstance.transform.SetParent(uiCanvas.transform);
        RectTransform dialogueBoxRect = dialogueInstance.GetComponent<RectTransform>();
        dialogueBoxRect.localPosition = Vector3.zero;
        dialogueBoxRect.localScale = new Vector3(1, 1, 1);
        
        dialogueText = dialogueInstance.GetComponentInChildren<TMP_Text>();
        dialogueBackground = dialogueInstance.GetComponentInChildren<Image>();

        WriteText(textToWrite);
    }

    public void LeftRange() {
        triggered = false;

        if (dialogueInstance != null) {
            Destroy(dialogueInstance);
            dialogueInstance = null;
        }
    }
}
