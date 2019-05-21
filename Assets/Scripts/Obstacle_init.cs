using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Obstacle_init : MonoBehaviour
{
    public static Tilemap tilemap;

    public static List<Vector3> accesibleTilePositionList = new List<Vector3>(); //Liste positions des tiles accessibles aux fourmis

    // Start is called before the first frame update
    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();//récupère l'objet tilemap.

        BoundsInt bounds = tilemap.cellBounds; //récupère le rectangle encadrant toutes les tiles disposées
        TileBase[] tileArray = tilemap.GetTilesBlock(bounds); //renvoi toutes les tiles comprises dans "bounds"
        GameObject[] prefabMap = tilemap.GetComponent<GameObject>(bounds);


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
                    switch(KeyValuePair){
                        case tile{

                            }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
