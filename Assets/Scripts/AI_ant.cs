using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System;

public class AI_ant : MonoBehaviour
{

    //private string facingDirection = "right";

    NavMeshAgent nav;
    //private Animator anim_fourmi;
    private bool m_reaching_point = false;
    private Vector3 dest = new Vector3(0,0,0);

    //private Animator animator_component;

    //protected readonly int has_a_direction = Animator.StringToHash("reaching_point");

    //method returning a new random destination
    private Vector3 pickRandomTile()
    {
        //récupération d'un nombre aléatoire correspondant à un index de la liste des tiles accessibles
        int randomNumber = (int)UnityEngine.Random.Range(0, Tiles_Monitor.TileofInterest.Count - 1);
        //récupéartion de la coordonnée globale de la tile choisie aléatoirement
        Vector3 globalTilePosition = Tiles_Monitor.TileofInterest[randomNumber];
        return globalTilePosition;
    }





    // Start is called before the first frame update
    void Start()
    {

        nav = GetComponent<NavMeshAgent>();
        //anim_fourmi = gameObject.transform.parent.Find("VisuFourmi").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (m_reaching_point == false){
            //choose randomly a destination
            dest = pickRandomTile();
            //Debug.Log("position de la tile destination :" + dest);
            nav.SetDestination(dest);
            m_reaching_point = true;
        }

        //condition d'arrivée à destination (ici 1px, peut être amené à être modifié)
        if (Math.Abs(dest.x- gameObject.transform.position.x) < 1 && Math.Abs(dest.y - gameObject.transform.position.y) < 1)
        {
            m_reaching_point = false;
        }
        
        GameObject.Find("VisuFourmi").transform.position = gameObject.transform.position;
        

    
    }
}
