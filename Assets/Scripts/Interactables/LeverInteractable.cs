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

            // theoretically flip the buttons          
            unLitRenderer.gameObject.SetActive(!_triggered);
            litRenderer.gameObject.SetActive(_triggered);
        } 
        get {
            return _triggered;
        }
    }

    PuzzleManager puzzleManager;
    public GameObject unlitSprite;
    SpriteRenderer unLitRenderer;

    public GameObject litSprite;
    SpriteRenderer litRenderer;
    DialogueBox dialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        puzzleManager = GetComponentInParent<PuzzleManager>();
        unLitRenderer = unlitSprite.GetComponent<SpriteRenderer>(); // assuming the thing itself is after the interaction key sprite
        litRenderer = litSprite.GetComponent<SpriteRenderer>();
        dialogueBox = GetComponent<DialogueBox>();

        triggered = false;
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
