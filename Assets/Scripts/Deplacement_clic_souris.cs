using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class deplacement : MonoBehaviour
{

    NavMeshAgent nav;
    

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
             //   if (!EventSystem.current.IsPointerOverGameObject())
                {
                    nav.SetDestination(hit.point);
                }
            }
        }
            GameObject.Find("VisuFourmi").transform.position = gameObject.transform.position;
    }
}
