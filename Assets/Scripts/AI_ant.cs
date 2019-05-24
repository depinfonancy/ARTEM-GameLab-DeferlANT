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

    NavMeshAgent nav;  //definition du NavMesh
    private Animator anim_fourmi;  //definition de l'Animator
    protected readonly int m_HashReachingPoint = Animator.StringToHash("reaching_point");  //Hash permettant de modifier l'état de l'animator (pour le booléen reaching point)
    private bool m_reaching_point = false;  //booléen indiquant si la fourmi est en train de rejoindre un point ou non
    private Vector3 dest = new Vector3(0, 0, 0);   //vecteur contenant la destination actuelle de la fourmi
    private bool facingRight = true;    // facing direction of the ant
    private NavMeshAgent navmeshFourmi;
    protected readonly int m_HashSpeedPara = Animator.StringToHash("speed");
    //method returning a new random destination
    private Vector3 pickRandomTile()
    {
        //récupération d'un nombre aléatoire correspondant à un index de la liste des tiles accessibles
        int randomNumber = (int)UnityEngine.Random.Range(0, Tiles_Monitor.TileofInterest.Count);
        //récupéartion de la coordonnée globale de la tile choisie aléatoirement
        Vector3 globalTilePosition = Tiles_Monitor.TileofInterest[randomNumber];
        return globalTilePosition;
    }

        private void Flip()
    {
        facingRight = !facingRight;

        // use negative scaling to reverse the sprite when the player is facing left
        Vector3 s = gameObject.transform.parent.Find("VisuFourmi").transform.localScale;
        s.x *= -1;
        gameObject.transform.parent.Find("VisuFourmi").transform.localScale = s;
    }


    // Start is called before the first frame update
    void Start()
    {
        //récupération des components
        nav = GetComponent<NavMeshAgent>();
        anim_fourmi = gameObject.transform.parent.Find("VisuFourmi").GetComponent<Animator>();
        navmeshFourmi = gameObject.transform.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //----------------MARCHE ALEATOIRE-----------------------------

        //Décision d'un point à atteindre lorsqu'il n'y en a pas
        if (m_reaching_point == false)
        {
            //choose randomly a destination
            dest = pickRandomTile();
            //Debug.Log("position de la tile destination :" + dest);
            nav.SetDestination(dest);
            m_reaching_point = true;
        }

        //condition d'arrivée à destination (ici 1px, peut être amené à être modifié)
        if (Math.Abs(dest.x - gameObject.transform.position.x) < 1 && Math.Abs(dest.y - gameObject.transform.position.y) < 1)
        {
            m_reaching_point = false;
        }
        //Mise à jour de l'Animator
        anim_fourmi.SetBool(m_HashReachingPoint, m_reaching_point);

        //Mise à jour de la position du visuel (VisuFourmi) pour qu'il suive l'agent
        gameObject.transform.parent.Find("VisuFourmi").transform.position = gameObject.transform.position;

        //------------------------------------------------------------------------
        if ((navmeshFourmi.velocity.x > 0 && !facingRight) || (navmeshFourmi.velocity.x < 0 && facingRight))
        {
            Flip();
        }

        float h = Input.GetAxis("Horizontal");
        anim_fourmi.SetFloat(m_HashSpeedPara, Mathf.Abs(h));
    }
}
