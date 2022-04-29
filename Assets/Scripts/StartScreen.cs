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

            // create char array to detect valid chars in channel name
            char[] channelCharList = new char[50];
            TMP_CharacterInfo[] cinfoList = channelNameTMPText.textInfo.characterInfo;

            int added_chars = 0;
            foreach (TMP_CharacterInfo cinfo in channelNameTMPText.textInfo.characterInfo) {
                // is the character alphanumeric or an underscore?
                if (char.IsLetterOrDigit(cinfo.character) || cinfo.character.Equals('_')) {
                    // yes! add it to our list
                    channelCharList[added_chars] = cinfo.character;
                    added_chars++;
                }
            }
            playerStats.ChannelName = channelCharList.ArrayToString();
        }

        if (nextScenePath != "") {
            SceneManager.LoadScene(nextScenePath);
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
