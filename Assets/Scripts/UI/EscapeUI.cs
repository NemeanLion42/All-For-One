using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseScreen;

    void Awake()
    {
        pauseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // toggle the pause screen
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseScreen.SetActive(!pauseScreen.activeInHierarchy);
        }
        
    }
}
