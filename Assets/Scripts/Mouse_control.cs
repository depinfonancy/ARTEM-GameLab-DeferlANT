using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Mouse_control : MonoBehaviour
{
    public GameObject PheromoneOrange;
    public TileBase brushTarget;

    //public float minimumInitialScale;
    private Vector3 posInit;
    GameObject pheromone;

    // marque quand le bouton de la souris est enfonce
    bool enCours = false;

    public Tilemap tilemap ;


    void Start()
    {
        //Tilemap tilemap = GetComponent<Tilemap>();
    }


    void Update()
    {
        GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        Vector3 posFin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // controle d'un petit curseur qui montre ou sera placee la prochaine pheromone (on limite les possibilites 
        // de placement pour essayer de rendre le code plus facile).
        Vector3Int cellPosition = gridLayout.WorldToCell(posFin);
        //Debug.Log(cellPosition);

        // paint une nouvelle case
        if (Input.GetMouseButtonDown(1))
        {
            tilemap.SetTile(cellPosition, brushTarget);
        }


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
