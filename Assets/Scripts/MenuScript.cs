using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
IMGUI est utilisé en attendant mais c'est très malpropre
On met un bouton transparent devant le bon sprite pour donner l'illusion mais ce n'ai pas du tout pratique
Il faut refaire le menu de manière plus propre en utilisant Unity.UI
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
        const int buttonWidth = 165;
        const int buttonHeight = 45;

        // On applique l'apparence
        GUI.skin = skin;

        // Affiche un bouton pour démarrer la partie
        if (
          GUI.Button(
            // Centré en x
            new Rect(Screen.width / 2 - (buttonWidth / 2),(Screen.height / 3) + (buttonHeight/2) -10,buttonWidth,buttonHeight),
            ""
          )
        )
        {
            // Sur le clic, on démarre le premier niveau
            // "Start_Scene" est le nom de la première scène que nous avons créés.
            UnityEngine.SceneManagement.SceneManager.LoadScene("Start_Scene");
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
