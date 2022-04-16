using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    // Camera Settings
    public float cameraEasing = 0.125f;
    public float cameraDesiredViewSize = 5;
    public float cameraAllowedPlayerAreaSize = 0.2f;

    // Player Settings
    public float playerMaxSpeed = 5;
    public float playerEasing = 0.25f;
    public KeyCode playerMoveUp = KeyCode.W;
    public KeyCode playerMoveDown = KeyCode.S;
    public KeyCode playerMoveLeft = KeyCode.A;
    public KeyCode playerMoveRight = KeyCode.D;
}
