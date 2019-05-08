using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // deplacement de l'ecran avec la souris
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    // limites de la carte
    public Vector2 panLimit;

    // reglages du zoom
    public float scrollSpeed = 20f;
    public float maxZoom = 5f;
    public float minZoom = 40f;




    void Update()
    {
        Vector3 pos = transform.position;
        float zoom = GetComponent<Camera>().orthographicSize;

        // haut
        if (Input.GetKey("s") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        // bas
        if (Input.GetKey("w") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        // gauche
        if (Input.GetKey("q") || Input.mousePosition.x <=panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        // droite
        if (Input.GetKey("x") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }


        // commande du zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * 100f * scrollSpeed * Time.deltaTime;

        // comparaison avec les limites, si c'est plus loin rien ne se passe
        zoom = Mathf.Clamp(zoom, maxZoom, minZoom);
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.y);
        pos.y = Mathf.Clamp(pos.y, -panLimit.y, panLimit.y);

        // actualisation des composants
        GetComponent<Camera>().orthographicSize = zoom;
        transform.position = pos;
    }
}
