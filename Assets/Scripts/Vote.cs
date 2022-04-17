using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Vote : MonoBehaviour, IGameCommand
{
    public string CommandString => "vote";
    public string ShortString => "v";

    public bool oneVotePerUser = false;

    private int votes1 = 0;
    private int votes2 = 0;
    private GameManager _gm;
    private ChatManager _cm;

    private HashSet<string> usersCountedInVote = new HashSet<string>();

    public bool Execute(string username, List<string> arguments, GameManager gm=null) {
        print(username + " sent the !vote command");

        if (oneVotePerUser && usersCountedInVote.Contains(username)) {
            return false; // vote was not counted
        } else {
            usersCountedInVote.Add(username);
        }

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

    public bool Execute(string username, List<string> arguments, ChatManager cm=null) {

        if (oneVotePerUser && usersCountedInVote.Contains(username)) {
            return false; // vote was not counted
        } else {
            usersCountedInVote.Add(username);
        }

        if (cm != null && arguments.Count > 0) {
            _cm = cm;

            if (arguments[0] == "1" || arguments[0].ToLower() == "one") {
                Votes1 += 1;
            } else if (arguments[0] == "2" || arguments[0].ToLower() == "two") {
                Votes2 += 1;
            }
        }

        return true;
    }

    public int Votes1 {
        get {
            return votes1;
        }
        set {
            votes1 = value;

            Debug.Log("+1 vote for option 1");
            if (_cm != null) {
                _cm.VotesForOne = value;
            }
        }
    }

    public int Votes2 {
        get {
            return votes2;
        }
        set {
            votes2 = value;

            Debug.Log("+1 vote for option 2");
            if (_cm != null) {
                _cm.VotesForTwo = value;
            }
        }
    }

    public void ResetVotes() {
        Votes1 = 0;
        Votes2 = 0;

        usersCountedInVote = new HashSet<string>();
    }
}
