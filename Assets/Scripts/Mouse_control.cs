using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_control : MonoBehaviour
{
    public GameObject PheromoneOrange;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SpawnPheromone();
        }
    }

    // faire apparaitre un cercle de pheromones a la pos cliquee de la souris
    void SpawnPheromone()
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pz.z = 0;
        GameObject bullet = Instantiate(PheromoneOrange, pz, Quaternion.identity);
    }

}
