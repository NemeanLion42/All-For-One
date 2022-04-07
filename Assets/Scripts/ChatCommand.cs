using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChatCommand
{
    public IGameCommand command;
    public string username;
    public DateTime timestamp;
    public List<string> arguments;



    public ChatCommand(ChatMessage message, IGameCommand submittedCommand) {
        arguments = new List<string>();
        command = submittedCommand;
        username = message.user; 
        timestamp = DateTime.Now; 
        ParseCommandArguments(message);
    }

    public void ParseCommandArguments(ChatMessage message) {
        String[] splitMessage = message.message.Split();

        if (splitMessage.Length > 1) {
            for (int i = 1; i < splitMessage.Length; i++) {
                arguments.Add(splitMessage[i]);
            }
        }
    }

    public bool Execute(GameManager gm) {
        return command.Execute(username, arguments, gm);
    }
}
