using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;

    public Vector2 panLimit;

    public float scrollSpeed = 20f;
    public float maxZoom = 5f;
    public float minZoom = 40f;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        float zoom = GetComponent<Camera>().orthographicSize;

        if (Input.GetKey("s") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("w") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("q") || Input.mousePosition.x <=panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("x") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * 100f * scrollSpeed * Time.deltaTime;

        zoom = Mathf.Clamp(zoom, maxZoom, minZoom);
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.y);
        pos.y = Mathf.Clamp(pos.y, -panLimit.y, panLimit.y);

        GetComponent<Camera>().orthographicSize = zoom;
        transform.position = pos;
    }
}
