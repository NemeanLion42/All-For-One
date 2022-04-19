using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public TMP_InputField channelNameInput;
    public PlayerStats playerStats;
    public string nextScenePath = "";
    TMP_Text channelNameTMPText;
    // Start is called before the first frame update
    void Start()
    {
        channelNameTMPText = channelNameInput.GetComponentsInChildren<TMP_Text>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {

        if (channelNameTMPText != null && playerStats != null) {
            playerStats.ChannelName = channelNameTMPText.text;
        }

        if (nextScenePath != "") {
            SceneManager.LoadScene(nextScenePath);
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
