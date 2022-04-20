using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DialogueInteractable : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnTriggerEnter2D(Collider2D collider2D) {
    //     Debug.Log("Entered!");
    // }

    void OnTriggerEnter2D(Collider2D collider2D) {
        Debug.Log("Entered!");
    }

    void OnTriggerExit2D(Collider2D collider2D) {
        Debug.Log("Exited :(");
    }

    void Execute() {
        
    }
}
