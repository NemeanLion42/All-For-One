using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeTracker : MonoBehaviour
{
    public List<Vector3> roomPositions;
    public List<GameObject> rooms;
    private AudioSource source;
    public AudioClip deathSound;
    public CameraController cameraController;
    public bool alive;
    public PlayerStats playerStats;
    int roomToShow = 0;
    float lastTimeMoved = 0;
    float timeBetweenMoves = 2;
    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        roomPositions = new List<Vector3>();
        rooms = new List<GameObject>();
        // roomPositions.Add(Vector3.zero);
        source = GetComponent<AudioSource>();        


        // Subscribe to the game over trigger
        PlayerStats.TriggerGameOver += OnGameOverTriggered;
    }

    private void OnGameOverTriggered(bool success)
    {
        // success bool tells you if the player won or lost the game.
        if (alive) {
            source.PlayOneShot(deathSound, 1.0f);
        }
        alive = false;
    }

    void OnDisable() {
        PlayerStats.TriggerGameOver -= OnGameOverTriggered;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive) {
            GameObject room = cameraController.currentRoom;
            if (room != null && (rooms.Count == 0 || room.transform.position != roomPositions[roomPositions.Count - 1])) {
                roomPositions.Add(room.transform.position);
                rooms.Add(room);
            }
        } else {
            if (Time.time > lastTimeMoved + timeBetweenMoves) {
                if (roomToShow < rooms.Count) {
                    cameraController.currentRoom = rooms[roomToShow];
                    lastTimeMoved = Time.time;
                    roomToShow++;
                } else {
                    Debug.Log("end");
                }
            }
        }
    }

    
}
