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

    public Text votesForOne;
    public Text votesForTwo;
    public Text votingState;
    public Text connectionState;

    private TwitchChat chat;
    private List<ChatCommand> newCommands;
    private IGameCommand[] gameCommands;

    private Vote voteScript;

    // Start is called before the first frame update
    void Start()
    {
        gameCommands = GetComponents<IGameCommand>();
        voteScript = gameObject.GetComponent<Vote>();

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
        } else {
            chat.ReadChat();
        }

        if (chat.connected) {
            connectionState.text = "Twitch connected";
            connectionState.color = Color.green;
        } else {
            connectionState.text = "Twitch not connected";
            connectionState.color = new Color(1, 130f/255f, 0);
        }

        
    }

    private void SetGameState(GameState newGameState) {
        gameState = newGameState;
    }


    public void ResetGame() {
        if (gameState != GameState.Playing) {
            SetGameState(GameState.Playing);

            if (voteScript != null) {
                voteScript.Votes1 = 0;
                voteScript.Votes2 = 0;
            }

            if (votingState != null) {
                votingState.text = "voting is open";
                votingState.color = Color.green;
            }
        }
    }

    public void EndGame() {
        if (gameState != GameState.GameOver) {
            SetGameState(GameState.GameOver);

            if (votingState != null) {
                votingState.text = "voting is closed";
                votingState.color = new Color(1, 130f/255f, 0);
            }
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
