using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;


public class AI_ant : MonoBehaviour
{

    //private string facingDirection = "right";

    NavMeshAgent nav;
    //private Animator anim_fourmi;
    private bool m_reaching_point = false;

    //private Animator animator_component;

    //protected readonly int has_a_direction = Animator.StringToHash("reaching_point");

    //method returning a new random destination
    private Vector3 pickRandomTile()
    {
        //récupération d'un nombre aléatoire correspondant à un index de la liste des tiles accessibles
        int randomNumber = (int)Random.Range(0, Tiles_Monitor.accesibleTilePositionList.Count - 1);
        //récupéartion de la coordonnée globale de la tile choisie aléatoirement
        Vector3 globalTilePosition = Tiles_Monitor.accesibleTilePositionList[0];
        return globalTilePosition;
    }



    // Start is called before the first frame update
    void Start()
    {

        nav = GetComponent<NavMeshAgent>();
        //anim_fourmi = gameObject.transform.parent.Find("VisuFourmi").GetComponent<Animator>();

        Vector3 pos = pickRandomTile();
        Debug.Log("position de la 1ère tile:"+pos);
    }

    // Update is called once per frame
    void Update()
    {
        



        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //   if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("hit.point" + hit.point);
                    //nav.SetDestination(hit.point);
                }
            }
        }
        //GameObject.Find("VisuFourmi").transform.position = gameObject.transform.position;
        

    
    }
}
