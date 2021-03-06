using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public PlayerStats playerStats;
    private string channelName;

    // ==== CONTROL FLOW ====
    public enum GenerationState {
        TwitchGen,
        RNG
    }

    public GenerationState currentGenState = GenerationState.RNG;


    // ==== TWITCH VARIABLES FOR CLIENT AND COMMANDS ====
    private TwitchClient twitchClient;
    private List<ChatCommand> commandsFromChat;
    private IGameCommand[] gameCommands;




    // ==== HANDLE VOTING ====
    private Vote voteScript;
    private int voteRoundNum = 1;
    private string[] votingOptions;

    void Awake()
    {
        // do we have a streamer channel?
        if (playerStats == null || playerStats.ChannelName.Equals("")) {
            // nope, switch to RNG mode and don't connnect to Twitch
            Debug.Log("Entered RNG mode!");
            currentGenState = GenerationState.RNG;
        } else {
            Debug.Log("Channel name is: "+playerStats.ChannelName);
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

            ConnectClient();
        }

        // Debug.Log("Open voting with 1; Close with 2");
        
    }

    // Update is called once per frame
    void Update() {
        // if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //     // 1 key was pressed down
        //     Debug.Log("Opening voting");
        //     StartVoting("sewer,charge port,home", ',');
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha2)) {
        //     // 2 key was pressed down
        //     Debug.Log("Closing vote and counting");
        //     Debug.Log("Winner: "+CountVotes());
        // }
    }
    void FixedUpdate()
    {
        switch(currentGenState) {
            case GenerationState.TwitchGen: {
                ProcessCommands();
                // temporaryTextBoxForLogging.text = VotesForOne.ToString() + " for One\n"+VotesForTwo.ToString() + " for Two";
            
                // if (twitchClient.clientConnected) {
                //     tempConnectedTextbox.text = "-- connected --";
                //     tempConnectedTextbox.color = new Color(0f, 0.6f, 0f);
                // } else {
                //     tempConnectedTextbox.text = "-- disconnected --";
                //     tempConnectedTextbox.color = Color.red;
                // }

            } break;
            case GenerationState.RNG: {

            } break;
        }        
    }

    public bool ConnectClient() {
        if (twitchClient == null) return false;

        switch (currentGenState) {
            case GenerationState.TwitchGen: {
                // Connect our client to Twitch!
                bool didWeConnect = twitchClient.Connect(channelName);

                // Add all of the callbacks
                twitchClient.client.OnMessageReceived += OnMessageReceived;
                twitchClient.client.OnChatCommandReceived += OnChatCommandReceived;

                return true;
                
            }
            case GenerationState.RNG: {
                Debug.Log("ChatManager: Cannot connect because we're in RNG mode");

            } break;
        }

        return false;
    }

    public bool DisconnectClient() {
        if (twitchClient == null || twitchClient.client == null) return false;

        if (currentGenState == GenerationState.TwitchGen) {
            twitchClient.client.OnMessageReceived -= OnMessageReceived;
            twitchClient.client.OnChatCommandReceived -= OnChatCommandReceived;

            twitchClient.client.Disconnect();
            
        }

        return true;
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
        // This will count the votes and return the winner
        string winning_room = "";

        voteRoundNum++;

        switch (currentGenState) {
            case GenerationState.TwitchGen: {
                winning_room = voteScript.CountVotes();

                // Jack's addition, feel free to change
                if (winning_room == null) winning_room = "0:"+votingOptions[UnityEngine.Random.Range(0, votingOptions.Length)];

            } break;
            case GenerationState.RNG: {
                int rand_idx = UnityEngine.Random.Range(0, votingOptions.Length);
                winning_room = "0:"+votingOptions[rand_idx];
            } break;
        }
        
        return winning_room;
    }

    public void StartVoting(string delimited_list, char delimiter) {
        // restarts the voting and defines the list of valid things to vote for
        string[] options = delimited_list.Split(delimiter);
        votingOptions = options;



        // Don't continue if using RNG
        if (currentGenState == GenerationState.RNG || voteRoundNum == 0) return;

        voteScript.SetVotingOptions(delimited_list, delimiter);

        string msg_to_send = "Voting round "+voteRoundNum+" has opened! Your options are: ";

        if (options.Length > 0) {
            for (int idx = 0; idx < options.Length - 1; idx++){
                msg_to_send += options[idx] + ", ";
            }

            // add the last option without a comma at the end
            msg_to_send += "or "+options[options.Length-1];
            msg_to_send += " -- Vote using !v or !vote followed by the room name; For more context on rooms, use !c or !context followed by a spaced list of room names with underscores instead of spaces for each room";

            SendMessageToChat(msg_to_send);
        }
    }


}
