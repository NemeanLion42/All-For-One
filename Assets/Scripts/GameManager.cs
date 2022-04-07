using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public enum GameState {
    Playing,
    GameOver
}


public class GameManager : MonoBehaviour
{
    public GameState gameState;
    public ChatCommand currentCommand;

    public Text recentMessage;

    private TwitchChat chat;
    private List<ChatCommand> newCommands;
    private IGameCommand[] gameCommands;


    // Start is called before the first frame update
    void Start()
    {
        gameCommands = GetComponents<IGameCommand>();

        newCommands = new List<ChatCommand>();
        chat = gameObject.GetComponent<TwitchChat>();

        SetGameState(GameState.Playing);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameState == GameState.Playing) {
            ChatMessage recentMessage = chat.ReadChat();

            IGameCommand command = CommandIsValid(recentMessage);
            if (command != null) {
                ChatCommand newCommand = new ChatCommand(recentMessage, command);
                newCommands.Add(newCommand);
            }

            ProcessCommands();
        }

        
    }

    private void SetGameState(GameState newGameState) {
        gameState = newGameState;
    }


    public void ResetGame() {
        if (gameState != GameState.Playing) {
            SetGameState(GameState.Playing);
        }
    }

    public void EndGame() {
        if (gameState != GameState.GameOver) {
            SetGameState(GameState.GameOver);
        }
    }

    private IGameCommand CommandIsValid(ChatMessage chatMessage) {
        if (chatMessage != null) {
            string commandString = chatMessage.message.Split()[0];

            foreach (IGameCommand command in gameCommands) {
                if (commandString == command.CommandString || commandString == command.ShortString) {
                    return command;
                }
            }
        }
        return null;
    }

    private void ProcessCommands() {
        if (newCommands.Count > 0) {
            newCommands[0].Execute(this);
            newCommands.RemoveAt(0);
        }
    }
}
