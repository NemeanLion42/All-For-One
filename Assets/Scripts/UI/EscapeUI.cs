using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EscapeUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseScreen;
    public GameObject gameOverScreen;
    public TMP_Text winText;
    public TMP_Text loseText;

    void Awake()
    {
        pauseScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        PlayerStats.TriggerGameOver += OnGameOver;

    }

    private void OnGameOver(bool success)
    {
        if (gameOverScreen != null) {
            gameOverScreen.SetActive(true);
            winText.gameObject.SetActive(success);
            loseText.gameObject.SetActive(!success);
        }
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
