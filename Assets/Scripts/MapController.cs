using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCamera;
    public Dictionary<string, Transform> roomTypes;
    public Transform[] roomPrefabs;
    public static int roomSize = 30;

    // connect to twitch on start

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
        // open poll with list of valid rooms
        // close poll returns a string
        return Instantiate(roomPrefabs[Mathf.FloorToInt(Random.Range(0, 2))], roomPosition, Quaternion.identity).gameObject;
    }
}
