using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Obstacle_init : MonoBehaviour
{
    public static Tilemap tilemap;
    public Grid grid;

    public GridBrushBase plein;

    public GridBrushBase salle;
    public GridBrushBase salled;
    public GridBrushBase salleg;

    public GridBrushBase virhd;
    public GridBrushBase virhg;
    public GridBrushBase virbd;
    public GridBrushBase virbg;
    public GridBrushBase vertical;
    public GridBrushBase horizontal;

    public GridBrushBase creuseh;
    public GridBrushBase creuseb;
    public GridBrushBase creused;
    public GridBrushBase creuseg;

    public GridBrushBase Tg;
    public GridBrushBase Td;
    public GridBrushBase Th;
    public GridBrushBase Tb;

    public GridBrushBase intersection;

    GridBrushBase[] liste_des_brush;
    
    public static List<Vector3> accesibleTilePositionList = new List<Vector3>(); //Liste positions des tiles accessibles aux fourmis

    // Start is called before the first frame update
    void Start()
    {
        liste_des_brush = new GridBrushBase[] { creuseh, creused, virhd, creuseb, vertical, virbd, Tg, creuseg, virhg, horizontal, Tb, virbg, Td, Th, intersection, salle, salled, salleg};

        Tilemap tilemap = GetComponent<Tilemap>();//récupère l'objet tilemap.

        BoundsInt bounds = tilemap.cellBounds; //récupère le rectangle encadrant toutes les tiles disposées
        TileBase[] tileArray = tilemap.GetTilesBlock(bounds); //renvoi toutes les tiles comprises dans "bounds"
        // = tilemap.GetComponent<GameObject>(bounds);


        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = tileArray[x + y * bounds.size.x];
                if (tile != null)
                {
                    //coordonnées locales de la tile
                    Vector3Int localTilePosition = new Vector3Int(x + bounds.xMin, y + bounds.yMin, 0);

                    //conversion en coordonnées globales pour le navmesh
                    Vector3 globalTilePosition = tilemap.GetCellCenterWorld(localTilePosition);

                    accesibleTilePositionList.Add(globalTilePosition);
                    //Debug.Log("AddedTile:" + tile.name);
                    /*switch(KeyValuePair){
                        case tile:
                            {

                            }
                    }*/
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // fonction appellee depuis mouse control, actualise les collider du navmesh
    public void actualise_brush(Vector3Int pos1, int[] brush) // on donne la position de la tile et la position de la tile dans la liste de tiles pour associer le bon brush
    {
        liste_des_brush[brush[0]].Paint(grid, tilemap.gameObject, pos1);
        liste_des_brush[brush[1]].Paint(grid, tilemap.gameObject, pos1 + new Vector3Int(0, 1, 0));
        liste_des_brush[brush[2]].Paint(grid, tilemap.gameObject, pos1 + new Vector3Int(1, 0, 0));
        liste_des_brush[brush[3]].Paint(grid, tilemap.gameObject, pos1 + new Vector3Int(0, -1, 0));
        liste_des_brush[brush[4]].Paint(grid, tilemap.gameObject, pos1 + new Vector3Int(-1, 0, 0));
    }

}
