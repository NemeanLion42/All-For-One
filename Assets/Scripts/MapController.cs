using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject player;
    public ChatManager chatManager;
    public GameObject mainCamera;
    public Transform startRoomPrefab;
    public Transform[] roomPrefabs;
    public static int roomSize = 30;

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
            if (room.position == roomPosition && room.gameObject != gameObject) return room.gameObject;
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

            startRoom = false;
            return newRoom;
        } else {
            string roomToGenerate = chatManager.CountVotes();
            Transform newRoomPrefab = null;
            foreach (Transform room in roomPrefabs) {
                if (room.gameObject.name.ToLower() == roomToGenerate) {
                    newRoomPrefab = room;
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
