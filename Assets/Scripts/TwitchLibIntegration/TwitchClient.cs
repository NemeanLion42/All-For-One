using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;

public class TwitchClient : MonoBehaviour
{
    // Client object is defined within the TwitchLib Library
    public Client client;
    private string channel_name = "pocato3rd"; // name of your personal Twitch account (lowercase)
    private string bot_username = "all_for_one_cms611";

    // Start is called before the first frame update
    void Start()
    {
        // we want this script to be running whenever the game is running
        // Application.runInBackground = true;

        // set up the bot and tell it which channel to join
        ConnectionCredentials credentials = new ConnectionCredentials("all_for_one_cms611", Secrets.bot_access_token);
        client = new Client();
        client.Initialize(credentials, channel_name);

        // here we will subscribe to any EVENTS we want our bot to listen for
        // TO BE FILLED IN LATER


        // connect our bot to the channel
        client.Connect();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            // if the 1 key is down, send a message!
            client.SendMessage(client.JoinedChannels[0], "This is a test message from the bot");

            GetComponent<SpriteRenderer>().color = Color.cyan;
        } else if (Input.GetKeyUp(KeyCode.Alpha1)) {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
