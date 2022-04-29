using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomUI : MonoBehaviour
{
    // Start is called before the first frame update

    string currentRoom;
    string twitchString = "}";

    TMP_Text currentRoomText;
    public PlayerStats playerStats;


    void Start()
    {
        currentRoomText = GetComponent<TMP_Text>();
        PlayerStats.OnCurrentRoomUpdate += OnCurrentRoomUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCurrentRoomUpdate(string roomName, int votesForRoom) {
        string useRoomName = "";

        // Debug.Log(roomName.Split('_').Length);

        foreach(string roomNamePiece in roomName.Split('_')) {
            useRoomName += roomNamePiece.Substring(0,1).ToUpper() + roomNamePiece.Substring(1) + " ";
        }

        string toWrite = useRoomName;
        if (votesForRoom > 0) {
            toWrite += twitchString+" ("+votesForRoom+")";
        } 

        currentRoomText.text = toWrite;
    }
}
