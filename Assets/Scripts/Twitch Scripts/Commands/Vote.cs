using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vote : MonoBehaviour, IGameCommand
{
    public string CommandString => "vote";
    public string ShortString => "v";

    public bool oneVotePerUser = false;

    private int votes1 = 0;
    private int votes2 = 0;

    private Dictionary<string, int>votes = new Dictionary<string, int>();

    private ChatManager _cm;

    private HashSet<string> usersCountedInVote = new HashSet<string>();

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

            string concat_args = "";
            foreach (string arg in arguments) {concat_args += arg + " ";}
            concat_args = concat_args.Trim();

            Debug.Log("Counting vote as: \""+concat_args+"\"");

            if (votes.ContainsKey(concat_args.Trim())) {
                Debug.Log("vote actually counted");
                votes[concat_args]++;
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

    public void SetVotingOptions(string delimited_list, char delimiter) {
        string[] listOfOptions = delimited_list.Split(delimiter);

        votes = new Dictionary<string, int>();
        foreach (string _opt in listOfOptions) {
            votes.Add(_opt, 0);
        }
    }

    public string CountVotes() {
        string max_key = null;
        int max_votes = 0;

        foreach (string k in votes.Keys) {
            int num_votes = votes[k];

            if (num_votes > max_votes) {
                max_key = k;
                max_votes = num_votes;
            }
        }


        return max_key;
    }
}
