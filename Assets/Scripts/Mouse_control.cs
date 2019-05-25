using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_control : MonoBehaviour
{
    public GameObject Pheromone;

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
            pheromone.transform.Find("Pheromone_Range").transform.localScale = dist * new Vector3(0.1f, 0.1f, 0.1f);

            //Ici il faut modifier le collider pour qu'il match avec le sprite, il faut trouver le bon rapport ou la bonne méthode
            /*
            CircleCollider2D collider = pheromone.transform.Find("Pheromone_Range").GetComponent<CircleCollider2D>();
            collider.radius = dist*0.1f;
            */
        }

    }



    // faire apparaitre un cercle de pheromones a la pos cliquee de la souris
    void SpawnPheromone()
    {
        posInit = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posInit.z = 0;
        pheromone = Instantiate(Pheromone, posInit, Quaternion.identity);
        pheromone.SetActive(true);
    }

}
