using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vote : MonoBehaviour, IGameCommand
{
    public string CommandString => "vote";
    public string ShortString => "v";

    public bool oneVotePerUser = false;

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

            string concat_args = "";
            foreach (string arg in arguments) {concat_args += arg + " ";}
            concat_args = concat_args.Trim().ToLower();
            if (votes.ContainsKey(concat_args)) {
                votes[concat_args]++;
            }
        }

        return true;
    }

    public void SetVotingOptions(string delimited_list, char delimiter) {
        string[] listOfOptions = delimited_list.Split(delimiter);

        votes = new Dictionary<string, int>();
        foreach (string _opt in listOfOptions) {
            votes.Add(_opt, 0);
        }

        usersCountedInVote = new HashSet<string>();
    }

    public string CountVotes() {
        string max_key = null;
        int max_votes = 0;

        string voteTallyString = "";

        foreach (string k in votes.Keys) {
            voteTallyString += k + ": " + votes[k] + "\n";
            int num_votes = votes[k];

            if (num_votes > max_votes) {
                max_key = num_votes.ToString() + ":" + k;
                max_votes = num_votes;
            }
        }
        // Debug.Log(voteTallyString);

        return max_key;
    }
}
