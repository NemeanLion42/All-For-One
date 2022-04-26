using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context : MonoBehaviour, IGameCommand
{
    public string CommandString => "context";
    public string ShortString => "c";

    

    float lastContextCmdReceived = -15.0f; // seconds
    float minTimeBetweenSending = 10.0f; // seconds

    float lastMsgSent = -1.0f;
    float timeBetweenOptions = 2.0f; // seconds


    // Handle the context table by reading in the file and updating contextHash
    public TextAsset roomContextsFile;
    private Hashtable contextHash = new Hashtable();

    List<string> contextMessagesToSend = new List<string>();

    ChatManager _cm;

    // Start is called before the first frame update
    void Start()
    {

        // Read the text file containing room contexts
        string roomContextContents = roomContextsFile.text;
        string[] listOfRooms = roomContextContents.Split('\n');

        foreach (string room_context in listOfRooms) {
            // is the line a comment?
            if (room_context.Substring(0, 2) != "//") {
                // no! add it to our table
                string[] name_msg_context = room_context.Split(':');
                contextHash.Add(name_msg_context[0], name_msg_context[1]);
            }
            
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Time.time > lastMsgSent + timeBetweenOptions && contextMessagesToSend.Count > 0) {
            string msg = contextMessagesToSend[0];

            _cm.SendMessageToChat(msg);
            contextMessagesToSend.RemoveAt(0);

            lastMsgSent = Time.time;
        }
        
    }

    public bool Execute(string username, List<string> arguments, ChatManager cm = null) {
        if (cm != null) {
            _cm = cm;
        }
        // have we waited long enough between context message to not get banned?
        if (Time.time > lastContextCmdReceived + minTimeBetweenSending) {
            // yes! update last sent time and send message
            lastContextCmdReceived = Time.time;

            if (cm != null) {
                bool we_have_args = (arguments.Count > 0) && (arguments[0] != "");

                // are they asking about specific rooms?
                if (we_have_args) {
                    // yes! check each arg and send it if the room exists
                    foreach (string arg in arguments) {

                        // does the room exist in our context table?
                        if (contextHash.ContainsKey(arg.ToLower())) {
                            string msg_to_send = (string)contextHash[arg.ToLower()];
                            contextMessagesToSend.Add(msg_to_send);
                        }
                    }

                } else {
                    // no! general check so just send all of the context messages
                    foreach (string contextKeys in contextHash.Keys) {
                        string msg_to_send = (string)contextHash[contextKeys];
                        contextMessagesToSend.Add(msg_to_send);
                    }
                }
            }
            
        }


        return true;
    }
}
