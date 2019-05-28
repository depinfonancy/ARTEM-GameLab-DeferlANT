using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
IMGUI est utilisé en attendant mais c'est pas très personnalisé 
On peux refaire le menu de manière plus stylé en utilisant Unity.UI
Ci dessous tuto d'1h expliquant comment faire ça bien
https://unity3d.com/fr/learn/tutorials/topics/user-interface-ui/creating-main-menu
    */


public class MenuScript : MonoBehaviour
{

    private GUISkin skin;

    // Start is called before the first frame update
    void Start()
    {
        // Chargement de l'apparence
        skin = Resources.Load("GUISkin") as GUISkin;
    }

    void OnGUI()
    {
        const int buttonWidth = 180;
        const int buttonHeight = 60;

        // On applique l'apparence
        GUI.skin = skin;

        // Affiche un bouton pour démarrer la partie
        if (
          GUI.Button(
            // Centré en x
            new Rect(Screen.width / 2 - (buttonWidth / 2),(Screen.height / 3) + (buttonHeight/2),buttonWidth,buttonHeight),
            "Nouvelle Partie"
          )
        )
        {
            // Sur le clic, on démarre le premier niveau
            // "Start_Scene" est le nom de la première scène que nous avons créés.
            UnityEngine.SceneManagement.SceneManager.LoadScene("Start_Scene");
        }

        if (
          GUI.Button(
            // Centré en x
            new Rect(Screen.width / 2 - (buttonWidth / 2), (Screen.height / 3) + 4*(buttonHeight/2), buttonWidth, buttonHeight),
            "Crédits"
          )
        )
        {
            // Sur le clic, on démarre le premier niveau
            // "Start_Scene" est le nom de la première scène que nous avons créés.
            UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
        }

        if (
          GUI.Button(
            new Rect(10, Screen.height -60, 100, 50),
            "Quit Game"
          )
        )
        {
            // Sur le clic, on démarre le premier niveau
            // "Start_Scene" est le nom de la première scène que nous avons créés.
            Application.Quit();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
