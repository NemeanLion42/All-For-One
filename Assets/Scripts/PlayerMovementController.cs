using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerMovementController : MonoBehaviour
{
    public SettingsController settings;
    private Rigidbody2D _rigidbody;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 intendedDirection = new Vector2();
        if (Input.GetKey(settings.playerMoveUp)) {
            intendedDirection += Vector2.up;
        }
        if (Input.GetKey(settings.playerMoveDown)) {
            intendedDirection += Vector2.down;
        }
        if (Input.GetKey(settings.playerMoveLeft)) {
            intendedDirection += Vector2.left;
        }
        if (Input.GetKey(settings.playerMoveRight)) {
            intendedDirection += Vector2.right;
        }

        Vector2 intendedVelocity = intendedDirection.normalized * settings.playerMaxSpeed;

        Vector2 difference = intendedVelocity - _rigidbody.velocity;
        _rigidbody.velocity += difference * settings.playerEasing;
    }
}
