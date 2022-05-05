using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TwitchNameUI : MonoBehaviour
{

    public PlayerStats playerStats;
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        text.text = playerStats.ChannelName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
