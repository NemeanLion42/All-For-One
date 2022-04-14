using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Vote : MonoBehaviour, IGameCommand
{
    public string CommandString => "!vote";
    public string ShortString => "!v";

    private int votes1 = 0;
    private int votes2 = 0;
    private GameManager _gm;

    public bool Execute(string username, List<string> arguments, GameManager gm=null) {
        print(username + " sent the !vote command");

        if (gm != null) {
            _gm = gm;

            if (arguments.Count >= 1) {
                if (arguments[0] == "1" || arguments[0].ToLower() == "one") {
                    Votes1 += 1;
                } else if (arguments[0] == "2" || arguments[0].ToLower() == "two") {
                    Votes2 += 1;
                }
            }

            // gm.recentMessageTMPro.GetComponent<TextMeshPro>().text = "Most recent message:\n"+arguments[0];
        }



        return true;
    }

    public int Votes1 {
        get {
            return votes1;
        }
        set {
            votes1 = value;

            if (_gm != null && _gm.votesForOne != null) {
                _gm.votesForOne.text = "Votes:\n"+votes1.ToString();
            }
        }
    }

    public int Votes2 {
        get {
            return votes2;
        }
        set {
            votes2 = value;

            if (_gm != null && _gm.votesForTwo != null) {
                _gm.votesForTwo.text = "Votes:\n"+votes2.ToString();
            }
        }
    }
}
