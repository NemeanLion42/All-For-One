using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Net.Sockets;

public class TwitchChat : MonoBehaviour
{

    public string username, password, channelName;

    public bool connected = false;

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    private float reconnectTimer, reconnectAfter;


    // Start is called before the first frame update
    void Start()
    {
        reconnectAfter = 60.0f;
        Connect();

    }

    // Update is called once per frame
    void Update()
    {
        connected = twitchClient.Connected;

        if (twitchClient.Available == 0) {
            reconnectTimer += Time.deltaTime;
        }

        if (twitchClient.Available == 0 && reconnectTimer >= reconnectAfter) {
            Connect();
            reconnectTimer = 0.0f;
        }

        // ReadChat(); (Vote's execute method takes care of this)
        
    }

    private void Connect() {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS "+password);
        writer.WriteLine("NICK "+username);
        writer.WriteLine("USER "+username+" 8 *:"+username);
        writer.WriteLine("JOIN #"+channelName);
        writer.Flush();
    }

    public ChatMessage ReadChat() {
        if (twitchClient.Available > 0) {
            string message = reader.ReadLine();

            if (message.Contains("PRIVMSG")) {

                // Get the username
                int splitPoint = message.IndexOf("!", 1);
                string chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                // Get the message itself
                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);

                // Fill the ChatMessage
                ChatMessage chatMessage = new ChatMessage();
                chatMessage.user = chatName;
                chatMessage.message = message.ToLower();
                
                return chatMessage;
            }
        }

        return null;
    }
}
