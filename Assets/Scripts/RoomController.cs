using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    MapController mapController;
    EnemyListController enemyListController;
    public GameObject player;
    public CameraController cameraController;
    // Start is called before the first frame update
    void Start()
    {
        mapController = GetComponentInParent<MapController>();
        enemyListController = GetComponentInChildren<EnemyListController>();
        player = mapController.player;
        cameraController = mapController.mainCamera.GetComponent<CameraController>();

    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject == cameraController.gameObject) {
            foreach (EnemyController enemy in enemyListController.GetComponentsInChildren<EnemyController>()) {
                if (enemy.state == EnemyController.EnemyState.UNAWARE) {
                    enemy.state = EnemyController.EnemyState.PURSUIT;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == cameraController.gameObject) {
            foreach (EnemyController enemy in enemyListController.GetComponentsInChildren<EnemyController>()) {
                enemy.state = EnemyController.EnemyState.UNAWARE;
            }
        }
    }
}
