using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    MapController mapController;
    // Start is called before the first frame update
    void Start()
    {
        mapController = gameObject.GetComponentInParent<MapController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // private void OnTriggerEnter2D(Collider2D other) {
    //     if (other.gameObject == mapController.player.gameObject) {
    //         mapController.mainCamera.GetComponent<CameraController>().currentRoom = gameObject;
    //     }
    // }
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject == mapController.player.gameObject) {
            mapController.mainCamera.GetComponent<CameraController>().currentRoom = gameObject;
        }
    }
}
