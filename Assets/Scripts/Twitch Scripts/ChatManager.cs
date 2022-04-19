using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;
    private string channelName;

    // ==== CONTROL FLOW ====
    public enum GenerationState {
        TwitchGen,
        RNG
    }
    public GenerationState currentGenState;


    // ==== TWITCH VARIABLES FOR CLIENT AND COMMANDS ====
    private TwitchClient twitchClient;
    private List<ChatCommand> commandsFromChat;
    private IGameCommand[] gameCommands;




    // ==== HANDLE VOTING ====
    private Vote voteScript;
    private int one_votes = 0, two_votes = 0;
    
    public int VotesForOne {
        get {
            return one_votes;
        }
        set {
            one_votes = value;
        }
    }
    public int VotesForTwo {
        get {
            return two_votes;
        }
        set {
            two_votes = value;
        }
    }


    // ==== TEMP TEXTBOXES TO SEE WHAT'S HAPPENING
    public Text temporaryTextBoxForLogging;
    public Text tempConnectedTextbox;


    // Start is called before the first frame update
    void Start()
    {
        // do we have a streamer channel?
        if (playerStats == null || playerStats.ChannelName.Equals("")) {
            // nope, switch to RNG mode and don't connnect to Twitch
            currentGenState = GenerationState.RNG;
        } else {
            currentGenState = GenerationState.TwitchGen;
            channelName = playerStats.ChannelName;

            twitchClient = GetComponent<TwitchClient>();
            if (twitchClient == null) {
                twitchClient = gameObject.AddComponent<TwitchClient>();
            }

            // Get all of the valid commands on the game object
            gameCommands = GetComponents<IGameCommand>();
            voteScript = GetComponent<Vote>();

            // Initialize a new list of the commands we've received
            commandsFromChat = new List<ChatCommand>();
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(currentGenState) {
            case GenerationState.TwitchGen: {
                ProcessCommands();
                temporaryTextBoxForLogging.text = VotesForOne.ToString() + " for One\n"+VotesForTwo.ToString() + " for Two";
            
                if (twitchClient.clientConnected) {
                    tempConnectedTextbox.text = "-- connected --";
                    tempConnectedTextbox.color = new Color(0f, 0.6f, 0f);
                } else {
                    tempConnectedTextbox.text = "-- disconnected --";
                    tempConnectedTextbox.color = Color.red;
                }

            } break;
            case GenerationState.RNG: {
                tempConnectedTextbox.text = "-- disconnected (RNG mode) --";
                tempConnectedTextbox.color = Color.red;

            } break;
        }
        
    }

    public void ConnectClient() {
        switch (currentGenState) {
            case GenerationState.TwitchGen: {
                // Connect our client to Twitch!
                twitchClient.Connect(channelName);

                // Add all of the callbacks
                twitchClient.client.OnMessageReceived += OnMessageReceived;
                twitchClient.client.OnChatCommandReceived += OnChatCommandReceived;
                
            } break;
            case GenerationState.RNG: {
                Debug.Log("ChatManager: Cannot connect because we're in RNG mode");
            } break;
        }
    }

    private void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
    {
        // This function is called every time we receive a command in chat

        // Create a ChatMessage to pass to the command checker
        ChatMessage recentMessage = new ChatMessage();
        recentMessage.user = e.Command.ChatMessage.Username;
        recentMessage.command = e.Command.CommandText;
        recentMessage.message = e.Command.ArgumentsAsString;

        IGameCommand command = CommandIsValid(recentMessage);
        if (command != null) {
            ChatCommand newCommand = new ChatCommand(recentMessage, command);
            commandsFromChat.Add(newCommand);
        }
        Debug.Log(e.Command.ChatMessage.Username+": !"+e.Command.CommandText+" - "+e.Command.ArgumentsAsString);
    }

    private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        // Debug.Log(e.ChatMessage.Username+": "+e.ChatMessage.Message);
        // I don't think we need to actually handle direct messages. We'll just handle commands
    }


    public void SendTestMessage() {
        SendMessageToChat("test message from ChatManager! "+UnityEngine.Random.Range(-10.0f, 10.0f));
    }

    public void SendMessageToChat(string message) {
        switch(currentGenState) {
            case GenerationState.TwitchGen: {
                twitchClient.SendMessageToChat(message);

            } break;
            case GenerationState.RNG: {

            } break;
        }

    }

    private IGameCommand CommandIsValid(ChatMessage chatMessage) {
        if (chatMessage != null) {
            string commandString = chatMessage.command;

            foreach (IGameCommand command in gameCommands) {
                if (commandString == command.CommandString || commandString == command.ShortString) {
                    return command;
                }
            }
        }
        return null;
    }

    private void ProcessCommands() {
        // Should this run through all and execute?

        if (commandsFromChat.Count > 0) {
            commandsFromChat[0].Execute(this);
            commandsFromChat.RemoveAt(0);
        }
    }

    public string CountVotes() {
        // This will reset the votes and return the winner
        Debug.Log("Counting votes... We have "+VotesForOne.ToString()+" for one; "+VotesForTwo.ToString()+" for two");
        voteScript.ResetVotes();

        return "Winner_room";
    }

    public void StartVoting(string delimited_list, char delimiter) {
        // starts the voting and defines the list of valid things to vote for
    }


}
