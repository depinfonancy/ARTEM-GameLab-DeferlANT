using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Menu_Script : MonoBehaviour
{

    private GameObject mouse_control;
    // Start is called before the first frame update
    void Start()
    {
        mouse_control = GameObject.Find("Mouse_control");
    }

    
    bool paused = false;
    const int buttonWidth = 180;
    const int buttonHeight = 60;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            paused = togglePause();
            mouse_control.GetComponent<Mouse_control>().enabled = false;
        }
    }
    
    void OnGUI()
    {
        if (paused)
        {
            GUI.Label(new Rect(Screen.width / 2 - 45, 20, 300, 500), "Game is paused!");

            if (GUI.Button(new Rect(Screen.width/2 - (buttonWidth/2), (Screen.height / 3), buttonWidth, buttonHeight), "Resume"))
            {
                paused = togglePause();
                mouse_control.GetComponent<Mouse_control>().enabled = true;
            }

            if (GUI.Button(new Rect(Screen.width / 2 - (buttonWidth / 2), (Screen.height / 3) + 2*(buttonHeight), buttonWidth, buttonHeight), "Menu"))
            {
                paused = togglePause();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            }
        }
    }

    bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }
}
