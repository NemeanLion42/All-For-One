using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Vote : MonoBehaviour, IGameCommand
{
    public string CommandString => "!vote";
    public string ShortString => "!v";

    public bool Execute(string username, List<string> arguments, GameManager gm=null) {
        print(username + " sent the !vote command");

        if (gm != null && gm.recentMessage != null) {

            if (arguments.Count >= 1) {
                gm.recentMessage.text = "Most recent message:\n\t"+arguments[0];
            }

            // gm.recentMessageTMPro.GetComponent<TextMeshPro>().text = "Most recent message:\n"+arguments[0];
        }



        return true;
    }
}
