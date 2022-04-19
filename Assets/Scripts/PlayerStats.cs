using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerStatsScriptableObj", order = 1)]
public class PlayerStats : ScriptableObject
{
    string channel_name = "pocato3rd";
    public string ChannelName {
        get {
            return channel_name;
        } 
        set {
            // we want to make sure that channel names are lower case and trimmed of leading/lagging whitespace
            channel_name = value.ToLower().Trim(); 
        }
    }
}
