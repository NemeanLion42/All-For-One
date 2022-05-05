using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ExitToMainMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    bool mouse_entered = false;
    TMP_Text text;
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {

        Color use_color = Color.white;
        if (mouse_entered) {
            if (text != null) {
                text.color = Color.white;
            }
        } else {
            if (text != null) {
                text.color = Color.white;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ChatManager chatManager = FindObjectOfType<ChatManager>();
        // chatManager.DisconnectClient();
        // SceneManager.LoadScene(0);
    }

    public void Clicked() {
        ChatManager chatManager = FindObjectOfType<ChatManager>();
        chatManager.DisconnectClient();
        SceneManager.LoadScene(0);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse has entered button!");
        mouse_entered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse has left button");
        mouse_entered = false;
    }


    
}
