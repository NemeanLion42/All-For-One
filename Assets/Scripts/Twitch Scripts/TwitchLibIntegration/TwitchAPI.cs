using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwitchLib.Unity;
using TwitchLib.Api.Core.Models.Undocumented.Chatters;
using System;

public class TwitchAPI : MonoBehaviour
{
    // The TwitchAPI class will allow us to make specific requests from the Twitch server (beyond just r/w messages in chat
    // We will be making use of callback functions because request/response is not immediate

    public Api api;
    public Client client;

    // Start is called before the first frame update
    void Start()
    {
        // Set to run in minimized mode
        Application.runInBackground = true;

        api = new Api();
        api.Settings.AccessToken = Secrets.bot_access_token;
        api.Settings.ClientId = Secrets.client_id;

        client = GetComponent<TwitchClient>().client;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2)) {

            if (client == null) {
                client = GetComponent<TwitchClient>().client;
            }

            api.Invoke(
                api.Undocumented.GetChattersAsync(client.JoinedChannels[0].Channel),
                GetChatterListCallback
            );
        }
    }

    private void GetChatterListCallback(List<ChatterFormatted> listOfChatters)
    {
        Debug.Log("List of " + listOfChatters.Count + " Viewers: ");
        foreach (var chatterObject in listOfChatters) {
            Debug.Log(chatterObject.Username);
        }
    }
}
