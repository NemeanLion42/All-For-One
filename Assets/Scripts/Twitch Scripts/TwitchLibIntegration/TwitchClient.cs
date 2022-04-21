using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;
using UnityEngine.UI;

public class TwitchClient : MonoBehaviour
{
    // Twitch Client connects our bot client to the specified channel. It allows us to send and receive messages
    // Client object is defined within the TwitchLib Library

    // Big credit to Honest Dan Games for their Beginners Guide to TwitchLib.Unity
    // Found at: https://docs.google.com/document/d/1GfYC3BGW2gnS7GmNE1TwMEdk0QYY2zHccxXp53-WiKM/edit 


    public Client client;
    public Text textbox;

    public float minTimeBetweenMessages = 1f;
    float lastTimeMessaged = -1f;
    List<string> messagesToSend = new List<string>();

    public bool clientConnected {
        get {
            return (client != null && client.IsConnected);
        }
    }

    private string channel_name = "pocato3rd"; // name of your personal Twitch account (lowercase)
    private string bot_username = "all_for_one_cms611";

    private int messageCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        // we want this script to be running whenever the game is running
        Application.runInBackground = true;       
        Debug.Log("Twitch Client initialized!");
    }


    public bool Connect() {

        if (client != null && client.IsConnected) {
            Debug.Log("Twitch already connected!");
            return true;
        }

        Debug.Log("Running the Twitch Client!");

        // set up the bot and tell it which channel to join
        ConnectionCredentials credentials = new ConnectionCredentials(bot_username, Secrets.bot_access_token);
        Debug.Log("Bot credentials initialized!");

        client = new Client();
        Debug.Log("new Client object made");

        client.Initialize(credentials, channel_name);
        Debug.Log("Client initialized");

        // here we will subscribe to any EVENTS we want our bot to listen for
        client.OnMessageReceived += Client_OnMessageReceived;

        // connect our bot to the channel
        client.Connect();

        Debug.Log("Client Connected!");
        
        return true;

    }

    public void Connect(string channelName) {

        channel_name = channelName;

        if (client != null && client.IsConnected) {
            Debug.Log("Twitch already connected!");
            return;
        }

        // set up the bot and tell it which channel to join
        ConnectionCredentials credentials = new ConnectionCredentials("all_for_one_cms611", Secrets.bot_access_token);

        client = new Client();
        client.Initialize(credentials, channelName.ToLower()); // need lower case chan name

        // here we will subscribe to any EVENTS we want our bot to listen for
        // client.OnMessageReceived += Client_OnMessageReceived;

        // connect our bot to the channel
        client.Connect();

        Debug.Log("Client connected!");
    }

    private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        Debug.Log(e.ChatMessage.Username + ": "+e.ChatMessage.Message);
    }



    // Update is called once per frame
    void Update()
    {
        if (messagesToSend.Count > 0 && lastTimeMessaged + Time.time > minTimeBetweenMessages) {
            lastTimeMessaged = Time.time;
            SendMessageToChat();
        }

    }

    public void SendTestMessage() {
        if (client != null && client.IsConnected) {
            client.SendMessage(client.JoinedChannels[0], "This is a test message from the bot" + messageCount.ToString());

            messageCount++;
        } else {
            Debug.Log("Can't send test message because client is not connected");
        }
    }

    private void SendMessageToChat() {
        if (client != null && client.IsConnected && messagesToSend.Count > 0) {
            string message = messagesToSend[0];
            messagesToSend.RemoveAt(0);
            

            if (client.JoinedChannels.Count > 0) {
                client.SendMessage(client.JoinedChannels[0], message);
            } else {
                Debug.Log("The client has not joined any channels :(( Attempting to join channel: "+channel_name);
                client.JoinChannel(channel_name);
            }
        }
    }

    public void SendMessageToChat(string message) {
        messagesToSend.Add(message);
    }

    public void QuitGame() {
        print("Quitting game!");
        Application.Quit();
    }
}
