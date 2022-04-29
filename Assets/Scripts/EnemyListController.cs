using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyListController : MonoBehaviour
{
    RoomController roomController;
    GameObject player;
    public Transform enemyPrefab;
    public int enemiesToSpawn;
    // Vector3[] spawnLocations = new Vector3[] {
    //     new Vector3(0, 0, 0),
    //     new Vector3(0, 0, 0),
    //     new Vector3(0, 0, 0)};
    // Start is called before the first frame update
    void Start()
    {
        // Create navmesh without logging because it creates a flood of messages
        Debug.unityLogger.logEnabled = false;
        transform.parent.GetComponentInChildren<UnityEngine.AI.NavMeshSurface2d>().BuildNavMesh();
        Debug.unityLogger.logEnabled = true;

        roomController = GetComponentInParent<RoomController>();
        player = roomController.player;
        // foreach (Vector3 loc in spawnLocations) {
        for (int i = 0; i < enemiesToSpawn; i++) {
            Transform newEnemy = Instantiate(enemyPrefab, transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0), Quaternion.identity);
            newEnemy.SetParent(transform);
            newEnemy.GetComponent<EnemyController>().target = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
