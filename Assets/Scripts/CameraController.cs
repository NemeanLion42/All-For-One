using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapController;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Camera _camera;
    public GameObject currentRoom;
    public float topBoundary;
    public float bottomBoundary;
    public float leftBoundary;
    public float rightBoundary;
    public Vector3 intendedPosition;
    public float easing = 0.125f;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Wait until the rooms have initialized
        if (currentRoom == null) return;
        // Calculate boundaries of current room and camera distances
        topBoundary = currentRoom.transform.position.y + roomSize/2;
        bottomBoundary = currentRoom.transform.position.y - roomSize/2;
        leftBoundary = currentRoom.transform.position.x - roomSize/2;
        rightBoundary = currentRoom.transform.position.x + roomSize/2;
        float halfHeight = _camera.orthographicSize;
        float halfWidth = halfHeight * _camera.aspect;
        float maxDistance = Mathf.Min(halfWidth, halfHeight) * 0.2f;

        // Follow Player
        Vector2 toPlayer = player.transform.position - transform.position;
        Vector2 intendedMovement = new Vector2();
        if (Mathf.Abs(toPlayer.x) > maxDistance) {
            intendedMovement += (Mathf.Abs(toPlayer.x)-maxDistance) * Vector2.right * Mathf.Sign(toPlayer.x);
        }
        if (Mathf.Abs(toPlayer.y) > maxDistance) {
            intendedMovement += (Mathf.Abs(toPlayer.y)-maxDistance) * Vector2.up * Mathf.Sign(toPlayer.y);
        }
        intendedPosition = new Vector3(transform.position.x + intendedMovement.x,
                                       transform.position.y + intendedMovement.y,
                                       transform.position.z);
        
        // Don't leave the map
        if (intendedPosition.x - halfWidth < leftBoundary) {
            intendedPosition.x = leftBoundary + halfWidth;
        } else if (intendedPosition.x + halfWidth > rightBoundary) {
            intendedPosition.x = rightBoundary - halfWidth;
        }
        if (intendedPosition.y - halfHeight < bottomBoundary) {
            intendedPosition.y = bottomBoundary + halfHeight;
        } else if (intendedPosition.y + halfHeight > topBoundary) {
            intendedPosition.y = topBoundary - halfHeight;
        }
        transform.position += (intendedPosition - transform.position) * easing;
    }
}
