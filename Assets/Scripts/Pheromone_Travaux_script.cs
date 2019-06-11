using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pheromone_Travaux_script : MonoBehaviour
{

    private CircleCollider2D cd;
    private NavMeshAgent nav;
    private AI_ant script;
    private Vector3 pheroPosition = new Vector3(0, 0, 0);
    private Vector3 newDest = new Vector3(0, 0, 0);
    Mouse_control mousecontrol;



    // Start is called before the first frame update
    void Start()
    {
        cd = GetComponent<CircleCollider2D>();
        pheroPosition = cd.transform.position;

        mousecontrol = GameObject.Find("Mouse_control").GetComponent<Mouse_control>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //vérifier que dans la table des collisions, "pheromone" n'interragit qu'avec "ant"
    void OnTriggerStay2D(Collider2D ant)
    {

        //Debug.Log("la fourmi " + ant.gameObject.name + " est dans la zone de la pheromone " + gameObject.name + " !");
        //Debug.Log("position de la cible : " +  pheroPosition);
        script = ant.GetComponent<AI_ant>();
        nav = ant.GetComponent<NavMeshAgent>();

        script.m_Triggered_by_Pheromone = true;
        script.m_reaching_point = false;

        Vector3 newDest = mousecontrol.findNearestAccesibleTile(pheroPosition);

        //ici marche que si la fourmi peut accéder à l'endroit où la phéromone est, il faut donc trouver la ile accessible la plus proche et la donner comme destination
        nav.SetDestination(newDest);

        



    }

   
}
