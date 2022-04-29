using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapController : MonoBehaviour
{
    public GameObject player;
    public ChatManager chatManager;
    public GameObject mainCamera;
    public Transform startRoomPrefab;
    public Transform[] roomPrefabs;
    public static int roomSize = 30;

    PlayerStats playerStats;
    private bool startRoom = true;

    private void CallStartVoting() {
        char delimiter = '\n';
        string[] names = new string[roomPrefabs.Length];
        for (int i = 0; i < names.Length; i++) {
            names[i] = roomPrefabs[i].gameObject.name;
        }
        string allNames = string.Join(delimiter.ToString(), names);
        chatManager.StartVoting(allNames.ToLower(), delimiter);
    }

    void Awake() {
        // should be run before Start()
        bool didClientConnect = chatManager.ConnectClient();
    }

    private void Start() {
        playerStats = AssetDatabase.LoadAssetAtPath<PlayerStats>("Assets/Scripts/StreamerStats.asset");
        CallStartVoting();
    }

    public GameObject MakeOrFindRoom(Vector3 playerPosition) {
        // Find where room should be
        int roomX = Mathf.RoundToInt(playerPosition.x/roomSize) * roomSize;
        int roomY = Mathf.RoundToInt(playerPosition.y/roomSize) * roomSize;
        Vector3 roomPosition = new Vector3(roomX, roomY, 0);
        // If there's a room there, return it
        Transform[] rooms = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform room in rooms) {
            if (room.position == roomPosition && room.gameObject != gameObject) {
                int votes = room.GetComponentInChildren<VoteTracker>().numVotes;

                playerStats.SetCurrentRoom(room.gameObject.name.Split('(')[0], votes);
                return room.gameObject;
            }
        }
        // If there's not, make a new room and return it

        GameObject newRoom = CreateRoom(roomPosition);
        newRoom.transform.SetParent(transform);
        return newRoom;
    }

    public GameObject CreateRoom(Vector3 roomPosition) {
        if (startRoom) {
            // chatManager.CountVotes();
            GameObject newRoom = Instantiate(startRoomPrefab, roomPosition, Quaternion.identity).gameObject;
            CallStartVoting();

            playerStats.SetCurrentRoom("Security", 0);

            startRoom = false;
            return newRoom;
        } else {
            string countedVotes = chatManager.CountVotes(); // {votes}:room_name
            string[] votesAndRoom = countedVotes.Split(':');
            int numVotes = 0;
            int.TryParse(votesAndRoom[0], out numVotes);
            string roomToGenerate = votesAndRoom[1];

            Transform newRoomPrefab = null;
            foreach (Transform room in roomPrefabs) {
                if (room.gameObject.name.ToLower() == roomToGenerate) {
                    newRoomPrefab = room;

                    VoteTracker voteTracker = newRoomPrefab.GetComponentInChildren<VoteTracker>();
                    voteTracker.numVotes = numVotes;

                    playerStats.SetCurrentRoom(roomToGenerate, numVotes);
                    break;
                }
            }
            if (newRoomPrefab == null) {
                throw new System.Exception("no room named " + roomToGenerate);
            }
            GameObject newRoom = Instantiate(newRoomPrefab, roomPosition, Quaternion.identity).gameObject;
            CallStartVoting();
            return newRoom;
        }
    }
}
