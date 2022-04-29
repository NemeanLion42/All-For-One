using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public bool randomCombo = false;
    LeverInteractable[] leverList;

    public int[] leverCombo = new int[] {1,2,3,4}; // hour numbers

    public GameObject hintInteractable;
    public GameObject keyPrefab;

    int[] leversPressedInOrder;
    int numOfLeversPressed = 0;

    string[] nonAnswerOptions = new string[] {"pset", "walk", "lunch", "read", "file reports", "coffee chat", "dance party", "pet cats", "plug in charge ports"};


    // Start is called before the first frame update
    void Start()
    {
        Random.seed = System.DateTime.Now.Millisecond;

        leverList = GetComponentsInChildren<LeverInteractable>();

        for (int idx = 0; idx < leverList.Length; idx++) {
            leverList[idx].lever_number = idx;
        }

        leversPressedInOrder = new int[leverCombo.Length];

        if (randomCombo) GenerateCombination();

        PopulateHintText();

        
        
    }

    void GenerateCombination() {

        leverCombo = new int[4];

        leverCombo[0] = Random.Range(1, 14-leverCombo.Length);

        for (int i = 1; i < leverCombo.Length; i++) {
            int temp = Random.Range(leverCombo[i-1]+1, 14 - (leverCombo.Length - i));
            leverCombo[i] = temp;
        }

        // for (int i = 0; i < leverCombo.Length; i++) {
        //     int temp = leverCombo[i];
        //     int randomIdx = Random.Range(i, leverCombo.Length);
        //     leverCombo[i] = leverCombo[randomIdx];
        //     leverCombo[randomIdx] = temp;
        // }
    }

    void ShuffleNonAnswerOptions() {
        for (int i = 0; i < nonAnswerOptions.Length; i++) {
            string temp = nonAnswerOptions[i];
            int randomIdx = Random.Range(i, nonAnswerOptions.Length);
            nonAnswerOptions[i] = nonAnswerOptions[randomIdx];
            nonAnswerOptions[randomIdx] = temp;
        }
    }

    bool ComboContains(int hourNum) {
        for (int combo_idx=0; combo_idx < leverCombo.Length; combo_idx++) {
            if (leverCombo[combo_idx] == hourNum) return true;
        }

        return false;
    }

    void PopulateHintText() {
        string toWrite = "";

        int idxOfNonAnswers = 0;

        for (int hour=1; hour <= 12; hour++) {
            if (ComboContains(hour)) {
                toWrite += hour.ToString() + ": maintenance";
            } else {
                toWrite += hour.ToString() + ": " + nonAnswerOptions[idxOfNonAnswers];
                idxOfNonAnswers++;
            }

            if (hour % 3 == 0) {
                toWrite += "\n";
            } else {
                toWrite += " / ";
            }
        }

        hintInteractable.GetComponent<DialogueBox>().textToWrite[1] = toWrite;


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TurnOnLever(int leverNumber) {

        leversPressedInOrder[numOfLeversPressed] = leverNumber+1;
        numOfLeversPressed++;

        Debug.Log("We've pressed "+numOfLeversPressed.ToString());

        // did they hit the number of levers needed for combo?
        if (numOfLeversPressed == leverCombo.Length) {
            // yes! check if the press order matches our combo
            bool matching_combo = true;
            for (int combo_idx=0; combo_idx < leverCombo.Length; combo_idx++) {
                if (leverCombo[combo_idx] != leversPressedInOrder[combo_idx]) {
                    matching_combo = false;
                }
            }

            // did they match the combo?
            if (matching_combo) {
                if (keyPrefab != null) {
                    GameObject keyInstance = Instantiate(keyPrefab, Vector3.zero, Quaternion.identity);
                    keyInstance.transform.SetParent(transform.parent); // set the parent to be the room's transform
                    keyInstance.transform.localPosition = Vector3.zero;
                }
                print("Success! spawn a key");
            } else {
                // no, the combo doesn't match, reset things
                leversPressedInOrder = new int[leverCombo.Length];
                numOfLeversPressed = 0;

                foreach (LeverInteractable lever in leverList) {
                    lever.ResetLever();
                }
            }

        }
    }
}
