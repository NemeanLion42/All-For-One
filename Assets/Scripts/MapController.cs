using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCamera;
    public Transform roomPrefab;
    public static int roomSize = 30;
    // Start is called before the first frame update
    void Start()
    {
        for (int y = 0; y < 3; y++) {
            for (int x = 0; x < 3; x++) {
                Instantiate(roomPrefab, new Vector3(x*roomSize, y*roomSize, 0), Quaternion.identity).SetParent(transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit2D(Collider2D other) {
        // if (other.gameObject == player) {
        //     Instantiate();
        // }
    }
}
