using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour
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
        const int buttonWidth = 60;
        const int buttonHeight = 30;

        // On applique l'apparence
        GUI.skin = skin;

     

        // Affiche un bouton pour revenir au menu
        if (
          GUI.Button(
            new Rect(10, 10, buttonWidth, buttonHeight),
            "Retour"
          )
        )
        {
            // Sur le clic, on démarre le premier niveau
            // "Start_Scene" est le nom de la première scène que nous avons créés.
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }


        //texte des crédits
        GUI.Label(new Rect(Screen.width/2 - 45, Screen.height/3 + 20, 300, 500),
            "Gibraël Bay\nThomas Calais\nPatrice Chanol\nBastien Eglem\nJulie Esclafert\nMina Hwang\nPierre Laffont\nMaxime Latgé\nCédric Piquet\nBenjamin Soulan\n\nOlivier Ageron\nCédric Zanni");
    }

      

    // Update is called once per frame
    void Update()
    {
        
    }
}
