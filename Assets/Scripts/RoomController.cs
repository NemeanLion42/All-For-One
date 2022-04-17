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
        if (other.gameObject == player.gameObject && other.isTrigger) {
            cameraController.currentRoom = gameObject;
        } else if (other.gameObject == cameraController.gameObject) {
            foreach (NavMeshAgentController enemy in enemyListController.GetComponentsInChildren<NavMeshAgentController>()) {
                if (enemy.state == NavMeshAgentController.EnemyState.UNAWARE) {
                    enemy.state = NavMeshAgentController.EnemyState.PURSUIT;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == cameraController.gameObject) {
            foreach (NavMeshAgentController enemy in enemyListController.GetComponentsInChildren<NavMeshAgentController>()) {
                enemy.state = NavMeshAgentController.EnemyState.UNAWARE;
            }
        }
    }
}
