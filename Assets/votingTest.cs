using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class votingTest : MonoBehaviour
{

    public InputField inputField;
    public ChatManager chatManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnOpenVotes() {
        string inputText = inputField.text.ToLower().Trim();

        chatManager.StartVoting(inputText, ',');
    }

    public void OnCloseVotes() {
        string winner = chatManager.CountVotes();

        Debug.Log(winner + " won the vote!");
    }
}
