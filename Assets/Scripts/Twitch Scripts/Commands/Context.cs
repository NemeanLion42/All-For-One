using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context : MonoBehaviour, IGameCommand
{
    public string CommandString => "context";
    public string ShortString => "c";


    float lastContextCmdReceived = -10.0f; // seconds
    float minTimeBetweenSending = 10.0f; // seconds

    float lastMsgSent = -1.0f;
    float timeBetweenOptions = 2.0f; // seconds


    List<string> contextMessagesToSend = new List<string>();

    ChatManager _cm;

    // Start is called before the first frame update
    void Start()
    {
        
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
                contextMessagesToSend.Add("Charge port: Healing station");
                contextMessagesToSend.Add("Weapons depot: Item store ");
            }
            
        }


        return true;
    }
}
