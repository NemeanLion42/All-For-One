using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapController;
using static SettingsController;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public GameObject currentRoom;
    public SettingsController settings;
    private Camera _camera;
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

        // Change camera size to keep one room on screen at a time
        if (_camera.aspect > 1) {
            // width is larger, so maximize height while remaining in the room
            _camera.orthographicSize = Mathf.Min(settings.cameraDesiredViewSize, roomSize/(2*_camera.aspect));
        } else {
            // height is larger, so maximize width while remaining in the room
            _camera.orthographicSize = Mathf.Min(settings.cameraDesiredViewSize / _camera.aspect, roomSize/2);

        }

        float halfHeight = _camera.orthographicSize;
        float halfWidth = halfHeight * _camera.aspect;
        float maxDistance = Mathf.Min(halfWidth, halfHeight) * settings.cameraAllowedPlayerAreaSize;
        GetComponent<BoxCollider2D>().size = new Vector2(halfWidth*1.9f, halfHeight*1.9f);

        // Follow Player
        Vector2 toPlayer = player.transform.position - transform.position;
        Vector2 intendedMovement = new Vector2();
        if (Mathf.Abs(toPlayer.x) > maxDistance) {
            intendedMovement += (Mathf.Abs(toPlayer.x)-maxDistance) * Vector2.right * Mathf.Sign(toPlayer.x);
        }
        if (Mathf.Abs(toPlayer.y) > maxDistance) {
            intendedMovement += (Mathf.Abs(toPlayer.y)-maxDistance) * Vector2.up * Mathf.Sign(toPlayer.y);
        }
        Vector3 intendedPosition = new Vector3(transform.position.x + intendedMovement.x,
                                       transform.position.y + intendedMovement.y,
                                       transform.position.z);
        
        // Calculate boundaries of current room
        float topBoundary = currentRoom.transform.position.y + roomSize/2;
        float bottomBoundary = currentRoom.transform.position.y - roomSize/2;
        float leftBoundary = currentRoom.transform.position.x - roomSize/2;
        float rightBoundary = currentRoom.transform.position.x + roomSize/2;

        // Don't let the camera leave the current room
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
        transform.position += (intendedPosition - transform.position) * settings.cameraEasing;
    }
}
