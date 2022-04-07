using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : MonoBehaviour
{
    public Transform player;
    private Camera _camera;
    public float topBoundary;
    public float bottomBoundary;
    public float leftBoundary;
    public float rightBoundary;
    public Vector3 intendedPosition;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
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
        transform.Translate(intendedMovement);

        // Don't leave the map
        intendedPosition = transform.position;
        if (transform.position.x - halfWidth < leftBoundary) {
            intendedPosition.x = leftBoundary + halfWidth;
        } else if (transform.position.x + halfWidth > rightBoundary) {
            intendedPosition.x = rightBoundary - halfWidth;
        }
        if (transform.position.y - halfHeight < bottomBoundary) {
            intendedPosition.y = bottomBoundary + halfHeight;
        } else if (transform.position.y + halfHeight > topBoundary) {
            intendedPosition.y = topBoundary - halfHeight;
        }
        transform.position = intendedPosition;
    }
}
