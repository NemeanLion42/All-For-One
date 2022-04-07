using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private KeyCode moveUp = KeyCode.W;
    private KeyCode moveDown = KeyCode.S;
    private KeyCode moveLeft = KeyCode.A;
    private KeyCode moveRight = KeyCode.D;
    public int playerSpeed = 5;
    public bool easing = true;
    public float easingRatio = 0.25f;
    private Rigidbody2D _rigidbody;
    // public Vector2 intendedVelocity;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 intendedDirection = new Vector2();
        if (Input.GetKey(moveUp)) {
            intendedDirection += Vector2.up;
        }
        if (Input.GetKey(moveDown)) {
            intendedDirection += Vector2.down;
        }
        if (Input.GetKey(moveLeft)) {
            intendedDirection += Vector2.left;
        }
        if (Input.GetKey(moveRight)) {
            intendedDirection += Vector2.right;
        }

        Vector2 intendedVelocity = intendedDirection.normalized * playerSpeed;

        if (easing) {
            Vector2 difference = intendedVelocity - _rigidbody.velocity;
            _rigidbody.velocity += difference * easingRatio;
        } else {
            _rigidbody.velocity = intendedVelocity;
        }
    }
}
