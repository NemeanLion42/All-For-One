using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context : MonoBehaviour, IGameCommand
{
    public string CommandString => "context";
    public string ShortString => "c";


    float lastContextMessageSent = -10.0f; // seconds
    float minTimeBetweenMessages = 10.0f; // seconds

    ChatManager _cm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Execute(string username, List<string> arguments, ChatManager cm = null) {

        // have we waited long enough between context message to not get banned?
        if (Time.time > minTimeBetweenMessages + lastContextMessageSent) {
            // yes! update last sent time and send message
            lastContextMessageSent = Time.time;

            if (cm != null) {
                cm.twitchClient.SendMessageToChat("Option Context:"+
                    "  Charge port: Healing station  "+
                    "  Weapons depot: Item store  ");
            }
            
        }


        return true;
    }
}
