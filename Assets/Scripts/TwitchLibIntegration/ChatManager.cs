using System;
using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    TwitchClient twitchClient;

    private List<ChatCommand> commandsFromChat;
    private IGameCommand[] gameCommands;


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


    public Text temporaryTextBoxForLogging;


    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessCommands();
        temporaryTextBoxForLogging.text = VotesForOne.ToString() + " for One\n"+VotesForTwo.ToString() + " for Two";
    }

    public void ConnectClient() {
        // Connect our client to Twitch!
        twitchClient.Connect(GetChannelName());

        // Add all of the callbacks
        twitchClient.client.OnMessageReceived += OnMessageReceived;
        twitchClient.client.OnChatCommandReceived += OnChatCommandReceived;

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

    void SendMessageToChat(string message) {
        twitchClient.SendMessageToChat(message);
    }

    string GetChannelName() {
        string tempChannelName = "POCATO3RD";

        // our TwitchClient needs a lower case channel name, but the client handles that
        return tempChannelName;
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

    public void CountVotes() {
        // This will reset the votes
        Debug.Log("Counting votes... We have "+VotesForOne.ToString()+" for one; "+VotesForTwo.ToString()+" for two");
        voteScript.ResetVotes();
    }
}
