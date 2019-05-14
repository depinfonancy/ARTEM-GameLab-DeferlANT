using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tiles_Monitor : MonoBehaviour
{
    public static Tilemap tilemap;
    public TileBase TilePlein; //Tile pleine à mettre en paramètre pour l'exclure de la liste des tiles accessibles
    public TileBase TileSalle;
    public TileBase TileSalle_gauche;
    public TileBase TileSalle_droite;
    public TileBase TileEn_cours_gauche;
    public TileBase TileEn_cours_droite;
    public TileBase TileEn_cours_bas;
    public TileBase TileEn_cours_haut;

    public static List<Vector3> accesibleTilePositionList = new List<Vector3>(); //Liste positions des tiles accessibles aux fourmis
    public static List<Vector3> TileofInterest = new List<Vector3>(); //Liste positions des tiles pour l'IA des fourmis


    // Start is called before the first frame update
    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        
        BoundsInt bounds = tilemap.cellBounds; //récupère le rectangle encadrant toutes les tiles disposées
        TileBase[] tileArray = tilemap.GetTilesBlock(bounds); //renvoi toutes les tiles comprises dans "bounds"

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = tileArray[x + y * bounds.size.x];
                if (tile != null && tile != TilePlein)
                {
                    //coordonnées locales de la tile
                    Vector3Int localTilePosition = new Vector3Int(x+bounds.xMin, y+bounds.yMin, 0);

                    //conversion en coordonnées globales pour le navmesh
                    Vector3 globalTilePosition = tilemap.GetCellCenterWorld(localTilePosition);
                    
                    accesibleTilePositionList.Add(globalTilePosition);
                    //Debug.Log("AddedTile:" + tile.name);
                    if (tile == TileSalle || tile == TileSalle_droite || tile == TileSalle_gauche || tile == TileEn_cours_bas || tile == TileEn_cours_droite || tile == TileEn_cours_gauche || tile == TileEn_cours_haut)
                    {
                        TileofInterest.Add(globalTilePosition);
                    }
                }
            }
        }

        //Debug.Log("accesibleTilePositionList.Count: " + accesibleTilePositionList.Count);
        /*
        foreach (Vector3 positionArray in accesibleTilePositionList)
        {
            Debug.Log("x : " + positionArray[0] + " y : " + positionArray[1]);
        }
        */



        //Affiche la taille de bounds en nombre de tile
        //Debug.Log("bounds.size.x : " + bounds.size.x + "\n bounds.size.y : " + bounds.size.y);

        //Affiche dans la console toutes les tiles : leurs positions et leurs noms
        //Le (0,0) est en bas à gauche
        /*
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = tileArray[x + y * bounds.size.x];
                if (tile != null)
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                }
                else
                {
                    Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                }
            }
        }
        */

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
