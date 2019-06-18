using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using System;

public class catastrophe : MonoBehaviour
{
    // prise en compte de TOUTES les tiles
    public TileBase plein;

    public Tilemap tilemap;

    public GameObject Pied;
    private Vector3Int piedPosition;
    private Animator animPied;
    // Start is called before the first frame update

    void Start()
    {
        animPied = Pied.GetComponent<Animator>();
    }

    void Update()
    {
        Vector3Int piedPosition = tilemap.WorldToCell(Pied.transform.position);
        if (piedPosition.y < 4)
        {
            int[] temp = { 0, 0, 0, 0, 0 };
            GetComponent<Obstacle_init>().actualise_brush(piedPosition, temp);
            tilemap.SetTile(piedPosition, plein);
            tilemap.SetTile(piedPosition + new Vector3Int(1, 0, 0), plein);
            tilemap.SetTile(piedPosition + new Vector3Int(-1, 0, 0), plein);
            tilemap.SetTile(piedPosition + new Vector3Int(-2, 0, 0), plein);
            GetComponent<Obstacle_init>().calcul_accessibles();
        }

    }
}
