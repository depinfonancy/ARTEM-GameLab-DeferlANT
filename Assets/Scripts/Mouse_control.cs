using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_control : MonoBehaviour
{
    public GameObject PheromoneOrange;

    //public float minimumInitialScale;
    private Vector3 posInit;
    GameObject pheromone;

    // marque quand le bouton de la souris est enfonce
    bool enCours = false;



    void Update()
    {
        // a l'appui sur le bouton gauche de la souris un cercle de pheromone apparait
        if (Input.GetMouseButtonDown(0))
        {
            enCours = true;
            SpawnPheromone();
        }


        // la pheromone en cours devient independante de la souris
        if (Input.GetMouseButtonUp(0))
        {
            enCours = false;
            pheromone = null;
        }


        // update de la taille de la pheromone avec la souris
        if (enCours)
        {
            Vector3 posFin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dist = Vector3.Distance(posFin, posInit);
            pheromone.transform.localScale = dist * new Vector3(1, 1, 1);
        }

    }



    // faire apparaitre un cercle de pheromones a la pos cliquee de la souris
    void SpawnPheromone()
    {
        posInit = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posInit.z = 0;
        pheromone = Instantiate(PheromoneOrange, posInit, Quaternion.identity);
        pheromone.SetActive(true);
    }

}
